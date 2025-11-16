# 📋 Plan Détaillé - Solution 3: Hybrid FormData Encryption

Plan complet d'implémentation pour encrypter les métadonnées FormData tout en laissant les fichiers en clair.

---

## 🎯 Vue d'Ensemble

**Objectif:** Encrypter uniquement les champs textuels d'un FormData, laisser les fichiers binaires non encryptés.

**Format envoyé:**
```
FormData {
  "encryptedMetadata": "base64_encrypted_json",  // ✅ Encrypté
  "files[]": Blob,                               // ❌ Non encrypté
}
```

**Exemple concret:**
```
POST /api/disbursements

FormData:
  encryptedMetadata: "AeS+GcM/xyz123..."  // Contient: { amount: 1000, currency: "USD", projectId: "abc", ... }
  documents[0]: File(invoice.pdf)
  documents[1]: File(contract.pdf)
```

---

## 🔧 Backend - Ce qu'il faut faire

### 1. Créer un Middleware Custom pour Décrypter FormData

**Fichier:** `src/Afdb.ClientConnection.Api/Middleware/FormDataDecryptionMiddleware.cs`

#### Responsabilités:

1. **Détecter** si la requête contient `multipart/form-data`
2. **Lire** le FormData de la requête
3. **Extraire** le champ `encryptedMetadata`
4. **Décrypter** ce champ avec `IPayloadEncryptionService`
5. **Parser** le JSON décrypté
6. **Reconstruire** un nouveau FormData avec:
   - Les valeurs décryptées comme champs individuels
   - Les fichiers originaux non touchés
7. **Remplacer** le body de la requête par le nouveau FormData

#### Défis:

**Défi 1: Lire FormData sans consommer le stream**
```csharp
// Le problème: une fois lu, le Request.Body ne peut plus être relu
var form = await context.Request.ReadFormAsync();

// Solution: Activer le buffering
context.Request.EnableBuffering();
```

**Défi 2: Reconstruire le FormData**
```csharp
// On ne peut pas modifier Request.Form directement (readonly)
// On doit recréer un nouveau stream multipart/form-data

// Options:
// A) Créer un nouveau MultipartFormDataContent
// B) Utiliser des HttpContext.Items pour passer les valeurs
// C) Utiliser un Model Binder custom
```

**Défi 3: Rebinding des propriétés**
```csharp
// Comment faire matcher les propriétés décryptées avec le Command?

// Décrypté: { "amount": 1000, "currency": "USD" }
// Command: CreateDisbursementCommand { Amount, Currency, Files }

// Solutions:
// 1. Utiliser HttpContext.Items (simple mais hacky)
// 2. Custom Model Binder (propre mais complexe)
// 3. Modifier le Controller pour accepter différent format
```

#### Structure du Middleware:

```csharp
public class FormDataDecryptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FormDataDecryptionMiddleware> _logger;

    public async Task InvokeAsync(
        HttpContext context,
        IPayloadEncryptionService encryptionService)
    {
        // 1. Vérifier si multipart/form-data
        if (!context.Request.HasFormContentType)
        {
            await _next(context);
            return;
        }

        // 2. Vérifier si endpoint nécessite décryptage
        var shouldDecrypt = DeterminerSiDecryptageNecessaire(context);
        if (!shouldDecrypt)
        {
            await _next(context);
            return;
        }

        // 3. Lire le FormData
        var form = await context.Request.ReadFormAsync();

        // 4. Extraire et décrypter "encryptedMetadata"
        if (!form.ContainsKey("encryptedMetadata"))
        {
            // Erreur: encryptedMetadata manquant
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Missing encryptedMetadata field"
            });
            return;
        }

        var encryptedMetadata = form["encryptedMetadata"].ToString();

        try
        {
            // 5. Décrypter
            var decryptedJson = encryptionService.DecryptJson(encryptedMetadata);
            var metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(decryptedJson);

            // 6. Stocker dans HttpContext.Items pour que le Controller y accède
            context.Items["DecryptedMetadata"] = metadata;
            context.Items["OriginalFiles"] = form.Files;

            // 7. Continuer le pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to decrypt FormData metadata");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Failed to decrypt metadata"
            });
        }
    }
}
```

**Problèmes avec cette approche:**
- Le model binding ASP.NET ne va pas récupérer automatiquement les valeurs de `HttpContext.Items`
- On doit modifier soit:
  - Les Controllers pour lire manuellement `HttpContext.Items`
  - Créer un Custom Model Binder

---

### 2. Option A: Modifier les Controllers (Simple mais pas propre)

**Changement dans DisbursementsController:**

```csharp
// AVANT
[HttpPost]
public async Task<ActionResult> CreateDisbursement(
    [FromForm] CreateDisbursementCommand command)
{
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetDisbursementById), new { id = result.Id }, result);
}

// APRÈS
[HttpPost]
public async Task<ActionResult> CreateDisbursement()
{
    // 1. Récupérer les métadonnées décryptées du middleware
    var metadata = HttpContext.Items["DecryptedMetadata"] as Dictionary<string, object>;
    var files = HttpContext.Items["OriginalFiles"] as IFormFileCollection;

    if (metadata == null)
    {
        return BadRequest(new { error = "Invalid encrypted metadata" });
    }

    // 2. Construire manuellement le Command
    var command = new CreateDisbursementCommand
    {
        Amount = Convert.ToDecimal(metadata["amount"]),
        Currency = metadata["currency"]?.ToString(),
        ProjectId = Guid.Parse(metadata["projectId"]?.ToString()),
        // ... mapper tous les champs
        Documents = files?.ToList()
    };

    // 3. Envoyer au Mediator
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetDisbursementById), new { id = result.Id }, result);
}
```

**Problèmes:**
- ❌ Perte du model binding automatique
- ❌ Perte de la validation automatique `[Required]`, `[Range]`, etc.
- ❌ Mapping manuel = code verbeux et error-prone
- ❌ Doit modifier TOUS les endpoints FormData (5 endpoints)

---

### 3. Option B: Custom Model Binder (Propre mais complexe)

**Créer:** `src/Afdb.ClientConnection.Api/ModelBinders/EncryptedFormDataModelBinder.cs`

```csharp
public class EncryptedFormDataModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var httpContext = bindingContext.HttpContext;

        // Récupérer les métadonnées décryptées du middleware
        var metadata = httpContext.Items["DecryptedMetadata"] as Dictionary<string, object>;
        var files = httpContext.Items["OriginalFiles"] as IFormFileCollection;

        if (metadata == null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        // Créer une instance du modèle
        var modelType = bindingContext.ModelType;
        var model = Activator.CreateInstance(modelType);

        // Mapper les propriétés du JSON décrypté vers le modèle
        foreach (var prop in modelType.GetProperties())
        {
            if (metadata.ContainsKey(ToCamelCase(prop.Name)))
            {
                var value = metadata[ToCamelCase(prop.Name)];
                var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                prop.SetValue(model, convertedValue);
            }
        }

        // Mapper les fichiers
        var filesProperty = modelType.GetProperty("Documents") ?? modelType.GetProperty("Files");
        if (filesProperty != null && files != null)
        {
            filesProperty.SetValue(model, files.ToList());
        }

        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}
```

**Utilisation dans le Controller:**

```csharp
[HttpPost]
public async Task<ActionResult> CreateDisbursement(
    [ModelBinder(typeof(EncryptedFormDataModelBinder))] CreateDisbursementCommand command)
{
    // Le command est déjà bindé automatiquement!
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetDisbursementById), new { id = result.Id }, result);
}
```

**Problèmes:**
- ⚠️ Complexe à implémenter (reflection, type conversion, nested objects)
- ⚠️ Gestion des types complexes (Guid, DateTime, decimal, etc.)
- ⚠️ Gestion des listes et objets nested
- ⚠️ Validation attributes ne s'appliquent pas automatiquement
- ⚠️ Doit gérer les cas edge (null, missing properties, etc.)

---

### 4. Option C: Attribute + Model Binder Provider (Plus propre)

**Créer un attribut custom:**

```csharp
[AttributeUsage(AttributeTargets.Parameter)]
public class FromEncryptedFormAttribute : Attribute, IModelNameProvider, IBindingSourceMetadata
{
    public BindingSource BindingSource => BindingSource.Custom;
    public string Name { get; set; }
}
```

**Utilisation:**

```csharp
[HttpPost]
public async Task<ActionResult> CreateDisbursement(
    [FromEncryptedForm] CreateDisbursementCommand command)
{
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetDisbursementById), new { id = result.Id }, result);
}
```

**Avantages:**
- ✅ Syntaxe propre (comme [FromForm], [FromBody])
- ✅ Explicite dans le code
- ✅ Réutilisable

**Nécessite aussi:**
- Le Model Binder (Option B)
- Un Model Binder Provider pour router vers le bon binder

---

### 5. Enregistrer le Middleware

**Dans Program.cs:**

```csharp
// Après PayloadDecryptionMiddleware (si existant)
app.UseMiddleware<FormDataDecryptionMiddleware>();
```

**Ordre important:**
```
Request
  ↓
SecurityHeadersMiddleware
  ↓
ExceptionHandlingMiddleware
  ↓
PayloadDecryptionMiddleware (pour JSON pur)
  ↓
FormDataDecryptionMiddleware (pour FormData)  // ← NOUVEAU
  ↓
Routing
  ↓
Authorization
  ↓
Controller
```

---

### 6. Modifier les Commands (Peut-être)

**Actuellement:**
```csharp
public class CreateDisbursementCommand : IRequest<CreateDisbursementResponse>
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public Guid ProjectId { get; set; }
    // ... autres propriétés

    public List<IFormFile>? Documents { get; set; }
}
```

**Possible changement:**
```csharp
public class CreateDisbursementCommand : IRequest<CreateDisbursementResponse>
{
    // Métadonnées (seront encryptées)
    public CreateDisbursementMetadata Metadata { get; set; }

    // Fichiers (non encryptés)
    public List<IFormFile>? Documents { get; set; }
}

public class CreateDisbursementMetadata
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public Guid ProjectId { get; set; }
    // ... autres propriétés
}
```

**Pourquoi?**
- Sépare clairement ce qui est encrypté vs non encrypté
- Plus facile pour le model binding

**Inconvénient:**
- Breaking change dans le code
- Doit modifier les handlers

---

## 🌐 Frontend - Ce qu'il faut faire

### 1. Créer une Fonction pour Préparer FormData Encrypté

**Fichier:** `utils/encryptedFormData.ts` (à créer)

```typescript
import { encrypt } from './encryption'; // Fonction d'encryption existante

interface EncryptedFormDataOptions {
  metadata: Record<string, any>;  // Les données à encrypter
  files?: {
    fieldName: string;  // Nom du champ FormData
    file: File;         // Le fichier
  }[];
}

export async function createEncryptedFormData(
  options: EncryptedFormDataOptions
): Promise<FormData> {
  const { metadata, files = [] } = options;

  // 1. Sérialiser les métadonnées en JSON
  const metadataJson = JSON.stringify(metadata);

  // 2. Encrypter le JSON
  const encryptedMetadata = await encrypt(metadataJson);

  // 3. Créer le FormData
  const formData = new FormData();

  // 4. Ajouter les métadonnées encryptées
  formData.append('encryptedMetadata', encryptedMetadata);

  // 5. Ajouter les fichiers (non encryptés)
  files.forEach(({ fieldName, file }) => {
    formData.append(fieldName, file);
  });

  return formData;
}
```

---

### 2. Modifier les Appels API

**Exemple pour CreateDisbursement:**

**AVANT:**
```typescript
async function createDisbursement(data: CreateDisbursementDto, files: File[]) {
  const formData = new FormData();

  // Ajouter les champs
  formData.append('amount', data.amount.toString());
  formData.append('currency', data.currency);
  formData.append('projectId', data.projectId);
  // ... tous les autres champs

  // Ajouter les fichiers
  files.forEach((file, index) => {
    formData.append(`documents[${index}]`, file);
  });

  const response = await fetch('/api/disbursements', {
    method: 'POST',
    body: formData
  });

  return response.json();
}
```

**APRÈS:**
```typescript
import { createEncryptedFormData } from '@/utils/encryptedFormData';

async function createDisbursement(data: CreateDisbursementDto, files: File[]) {
  // 1. Séparer métadonnées et fichiers
  const metadata = {
    amount: data.amount,
    currency: data.currency,
    projectId: data.projectId,
    // ... tous les champs sauf les fichiers
  };

  // 2. Créer FormData encrypté
  const formData = await createEncryptedFormData({
    metadata,
    files: files.map((file, index) => ({
      fieldName: `documents[${index}]`,
      file
    }))
  });

  // 3. Envoyer
  const response = await fetch('/api/disbursements', {
    method: 'POST',
    body: formData
  });

  return response.json();
}
```

---

### 3. Gérer la Réponse Encryptée

**La réponse sera toujours encryptée** (si l'endpoint est configuré pour):

```typescript
async function createDisbursement(data: CreateDisbursementDto, files: File[]) {
  const formData = await createEncryptedFormData({ metadata, files });

  const response = await fetch('/api/disbursements', {
    method: 'POST',
    body: formData
  });

  const encryptedResponse = await response.json();

  // Décrypter la réponse
  const decryptedData = await decrypt(encryptedResponse.encryptedData);

  return JSON.parse(decryptedData);
}
```

---

### 4. Adapter TOUS les Endpoints FormData

Il faudra modifier:

1. **POST /api/disbursements** (CreateDisbursement)
2. **PUT /api/disbursements/{id}** (UpdateDisbursement)
3. **POST /api/disbursements/{id}/submit** (SubmitDisbursement)
4. **POST /api/disbursements/{id}/resubmit** (ReSubmitDisbursement)
5. **POST /api/disbursements/{id}/back-to-client** (BackToClientDisbursement)

Chacun devra:
- Utiliser `createEncryptedFormData()`
- Passer les métadonnées vs fichiers séparément

---

## 📊 Récapitulatif des Changements

### Backend

| Fichier | Action | Complexité |
|---------|--------|-----------|
| `Middleware/FormDataDecryptionMiddleware.cs` | CRÉER | ⭐⭐⭐⭐ |
| `ModelBinders/EncryptedFormDataModelBinder.cs` | CRÉER | ⭐⭐⭐⭐⭐ |
| `Attributes/FromEncryptedFormAttribute.cs` | CRÉER | ⭐⭐ |
| `Program.cs` | MODIFIER | ⭐ |
| `Controllers/DisbursementsController.cs` | MODIFIER (5 endpoints) | ⭐⭐⭐ |
| `Commands/*.cs` | POSSIBLEMENT MODIFIER | ⭐⭐ |

**Total Backend:** ~6-8 heures de développement

### Frontend

| Fichier | Action | Complexité |
|---------|--------|-----------|
| `utils/encryptedFormData.ts` | CRÉER | ⭐⭐ |
| `api/disbursements.ts` (ou équivalent) | MODIFIER | ⭐⭐⭐ |
| Tous les composants appelant ces APIs | MODIFIER | ⭐⭐ |

**Total Frontend:** ~3-4 heures de développement

---

## ⚠️ Difficultés et Pièges

### Difficulté 1: Model Binding Complexe

**Problème:**
Le model binding ASP.NET ne comprend pas nativement comment mapper `HttpContext.Items` vers un Command.

**Solutions:**
- Custom Model Binder (complexe)
- Mapping manuel dans controller (verbeux)
- Modifier la structure des Commands (breaking change)

### Difficulté 2: Types Complexes

**Problème:**
Le JSON décrypté contient des `object`, il faut les convertir vers les bons types:
- `decimal` pour Amount
- `Guid` pour ProjectId
- `DateTime` pour dates
- Types nullable
- Listes et objets nested

**Solution:**
Utiliser un serializer robuste + gestion d'erreurs

### Difficulté 3: Validation

**Problème:**
Les attributs de validation `[Required]`, `[Range]`, etc. ne s'appliquent pas automatiquement si on fait du mapping manuel.

**Solution:**
Appeler manuellement la validation:
```csharp
var validationContext = new ValidationContext(command);
var validationResults = new List<ValidationResult>();
bool isValid = Validator.TryValidateObject(command, validationContext, validationResults, true);
```

### Difficulté 4: Files avec Noms Dynamiques

**Problème:**
Les fichiers peuvent avoir des noms comme:
- `documents[0]`, `documents[1]`, ...
- `additionalDocuments[]`

Comment les mapper vers `List<IFormFile>`?

**Solution:**
Parser les noms de champs et regrouper par préfixe.

### Difficulté 5: Debugging

**Problème:**
Avec l'encryption + middleware custom, il devient difficile de voir ce qui se passe.

**Solution:**
- Logs détaillés dans le middleware
- Mode debug pour désactiver temporairement l'encryption
- Outils pour voir le contenu décrypté

---

## 🧪 Tests Nécessaires

### Backend

1. **Test Middleware:**
   - Requête avec `encryptedMetadata` valide → OK
   - Requête sans `encryptedMetadata` → 400 Bad Request
   - `encryptedMetadata` invalide (pas base64) → 400
   - `encryptedMetadata` décryptable mais pas JSON → 400
   - Requête avec fichiers uniquement → Doit passer sans erreur

2. **Test Model Binder:**
   - Tous les types supportés (string, int, decimal, Guid, DateTime)
   - Types nullable
   - Listes
   - Objets nested
   - Fichiers

3. **Test Integration:**
   - CreateDisbursement avec métadonnées + fichiers
   - UpdateDisbursement avec métadonnées + fichiers
   - SubmitDisbursement avec fichiers uniquement

### Frontend

1. **Test encryptedFormData utility:**
   - Métadonnées uniquement
   - Fichiers uniquement
   - Métadonnées + fichiers
   - Types complexes (dates, nombres, null)

2. **Test E2E:**
   - Créer disbursement complet
   - Upload gros fichiers
   - Upload multiples fichiers
   - Gestion erreurs network

---

## 📈 Estimation de Temps

### Développement

- **Backend Middleware:** 4-6 heures
- **Backend Model Binder:** 3-4 heures
- **Backend Controller Changes:** 2-3 heures
- **Frontend Utility:** 1-2 heures
- **Frontend API Changes:** 2-3 heures

**Total Développement:** ~15-20 heures

### Tests

- **Backend Unit Tests:** 3-4 heures
- **Backend Integration Tests:** 2-3 heures
- **Frontend Tests:** 2-3 heures
- **E2E Tests:** 2-3 heures

**Total Tests:** ~10-15 heures

### Documentation + Review

- **Documentation:** 2-3 heures
- **Code Review:** 2-3 heures
- **Refactoring:** 2-4 heures

**Total:** ~6-10 heures

---

## 🎯 TOTAL ESTIMÉ: 30-45 heures (4-6 jours)

---

## 🚨 Recommandation Finale

**Cette solution est COMPLEXE** et nécessite:
- ⚠️ Middleware custom sophistiqué
- ⚠️ Model Binder custom avec reflection
- ⚠️ Changements dans tous les endpoints FormData
- ⚠️ Tests extensifs
- ⚠️ 30-45 heures de développement

**Alternatives plus simples:**
- **Solution 1:** NeverEncryptEndpoints (5 minutes)
- **Solution 5:** [NoEncryption] (15 minutes)

**Sauf si tu as vraiment besoin d'encrypter les métadonnées textuelles au niveau applicatif,** je recommande fortement Solution 1 ou 5.

---

**Veux-tu toujours implémenter Solution 3?** 🤔

Si oui, je commence par quel composant? (Middleware, Model Binder, ou Frontend utility?)
