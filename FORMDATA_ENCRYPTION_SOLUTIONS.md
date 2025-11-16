# 🔐 Solutions pour Encryption avec FormData + Fichiers

Analyse des solutions possibles pour gérer l'encryption des endpoints avec `FormData` et `IFormFile`.

---

## 📋 Le Problème

Les endpoints suivants utilisent `[FromForm]` avec des fichiers:

1. `POST /api/disbursements` - CreateDisbursementCommand + IFormFile
2. `PUT /api/disbursements/{id}` - EditDisbursementCommand + IFormFile
3. `POST /api/disbursements/{id}/submit` - List<IFormFile>
4. `POST /api/disbursements/{id}/resubmit` - string + List<IFormFile>
5. `POST /api/disbursements/{id}/back-to-client` - BackToClientDisbursementCommand

**Problème:** `FormData` ne peut pas être sérialisé en JSON car il contient des fichiers binaires.

---

## 🎯 Solution 1: Exclure les Endpoints FormData de l'Encryption (RECOMMANDÉ)

### Description

Ne pas encrypter les endpoints qui utilisent `multipart/form-data`. Les fichiers sont déjà transmis via HTTPS qui fournit l'encryption en transit.

### Configuration

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Global",
    "NeverEncryptEndpoints": [
      "/api/disbursements"  // Tous les endpoints disbursements
    ]
  }
}
```

### Frontend

```typescript
// Aucun changement nécessaire!
const formData = new FormData();
formData.append('amount', '1000');
formData.append('file', fileBlob);

await fetch('/api/disbursements', {
  method: 'POST',
  body: formData  // ✅ Envoi normal
});
```

### Backend

```csharp
// Aucun changement nécessaire!
[HttpPost]
public async Task<ActionResult> CreateDisbursement(
    [FromForm] CreateDisbursementCommand command)
{
    // Fonctionne normalement
}
```

### ✅ Avantages

1. **Simple** - Aucun changement de code nécessaire
2. **Performant** - Pas d'overhead d'encryption sur les fichiers
3. **Standard** - Utilise HTTPS pour la sécurité en transit
4. **Maintenable** - Pas de logique complexe
5. **Rétrocompatible** - Pas de breaking change

### ❌ Inconvénients

1. **Pas d'encryption payload** - Les métadonnées (amount, currency, etc.) ne sont pas encryptées au niveau applicatif
2. **Dépend de HTTPS** - La sécurité repose uniquement sur TLS
3. **Pas de end-to-end encryption** - Pas d'encryption au-delà du transport

### 💡 Use Case

- **Dev/Test**: Parfait
- **Production avec HTTPS strict**: Acceptable
- **Données ultra-sensibles**: Insuffisant
- **Compliance basique**: OK

### 🎯 Verdict

**⭐⭐⭐⭐⭐** - Solution recommandée pour la majorité des cas.

---

## 🎯 Solution 2: Séparer Métadonnées et Fichiers (2 Appels)

### Description

Diviser chaque endpoint en deux:
1. Endpoint JSON pour les métadonnées (encryptées)
2. Endpoint multipart pour les fichiers (non encryptés)

### Configuration

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Global",
    "NeverEncryptEndpoints": [
      "/api/disbursements/*/files"  // Uniquement les fichiers
    ]
  }
}
```

### Changements Backend

```csharp
// AVANT (1 endpoint)
[HttpPost]
public async Task<ActionResult> CreateDisbursement(
    [FromForm] CreateDisbursementCommand command)

// APRÈS (2 endpoints)
[HttpPost]
[EncryptedPayload]  // ✅ Encrypté
public async Task<ActionResult> CreateDisbursement(
    [FromBody] CreateDisbursementCommand command)

[HttpPost("{id}/files")]  // ❌ Non encrypté
public async Task<ActionResult> UploadFiles(
    Guid id,
    [FromForm] List<IFormFile> files)
```

### Changements Frontend

```typescript
// AVANT (1 appel)
const formData = new FormData();
formData.append('amount', '1000');
formData.append('file', fileBlob);
await fetch('/api/disbursements', { method: 'POST', body: formData });

// APRÈS (2 appels)
// 1. Créer disbursement avec métadonnées encryptées
const encryptedPayload = await encrypt({ amount: 1000, currency: 'USD' });
const response = await fetch('/api/disbursements', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ encryptedData: encryptedPayload })
});
const { id } = await response.json();

// 2. Upload fichiers (non encryptés)
const formData = new FormData();
formData.append('file', fileBlob);
await fetch(`/api/disbursements/${id}/files`, {
  method: 'POST',
  body: formData
});
```

### ✅ Avantages

1. **Métadonnées encryptées** - Les données sensibles sont protégées
2. **Fichiers non touchés** - Pas de transformation des binaires
3. **Granularité** - Contrôle fin sur ce qui est encrypté
4. **Sécurité accrue** - Meilleure que Solution 1

### ❌ Inconvénients

1. **Complexe** - Nécessite des changements importants
2. **2 appels réseau** - Impact performance
3. **Breaking change** - Frontend doit être entièrement refait
4. **Gestion d'erreurs** - Que faire si le 2e appel échoue?
5. **Atomicité** - Pas transactionnel (besoin de compensation)

### 💡 Use Case

- **Données ultra-sensibles**: Excellent
- **Compliance stricte**: Recommandé
- **Dev rapide**: Non
- **Migration progressive**: Difficile

### 🎯 Verdict

**⭐⭐⭐** - Bonne solution si vraiment nécessaire, mais complexe.

---

## 🎯 Solution 3: Encrypter Uniquement les Fields (Hybrid)

### Description

Encrypter les champs textuels de FormData, laisser les fichiers en clair. Nécessite un middleware custom.

### Implémentation

**Frontend:**
```typescript
const formData = new FormData();

// Encrypt les métadonnées
const metadata = { amount: 1000, currency: 'USD' };
const encrypted = await encrypt(metadata);
formData.append('encryptedMetadata', encrypted);

// Fichiers en clair
formData.append('file', fileBlob);

await fetch('/api/disbursements', {
  method: 'POST',
  body: formData
});
```

**Backend Middleware:**
```csharp
public class FormDataDecryptionMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.HasFormContentType)
        {
            var form = await context.Request.ReadFormAsync();

            // Décrypte le champ "encryptedMetadata"
            if (form.ContainsKey("encryptedMetadata"))
            {
                var encrypted = form["encryptedMetadata"];
                var decrypted = _encryptionService.DecryptJson(encrypted);

                // Injecte les valeurs décryptées dans le form
                // ... logique custom
            }
        }

        await _next(context);
    }
}
```

**Backend Controller:**
```csharp
[HttpPost]
public async Task<ActionResult> CreateDisbursement(
    [FromForm] string encryptedMetadata,  // ✅ Encrypté
    [FromForm] List<IFormFile> files)      // ❌ Non encrypté
{
    // Décrypté par le middleware
}
```

### ✅ Avantages

1. **1 seul appel** - Pas de split
2. **Métadonnées protégées** - Données sensibles encryptées
3. **Fichiers performants** - Pas de transformation binaire

### ❌ Inconvénients

1. **Très complexe** - Middleware custom sophistiqué
2. **Breaking change** - Frontend doit changer
3. **Binding difficile** - Comment rebinder les propriétés décryptées?
4. **Maintenabilité** - Logique custom non standard
5. **Bugs potentiels** - Beaucoup de code custom

### 💡 Use Case

- **Si vraiment besoin d'encryption + 1 appel**: OK
- **Team expérimentée**: Peut-être
- **Production immédiate**: Non

### 🎯 Verdict

**⭐⭐** - Trop complexe pour la valeur ajoutée.

---

## 🎯 Solution 4: Encrypter les Fichiers en Base64 (Brutal Force)

### Description

Convertir les fichiers en base64, les encrypter avec le reste, puis les décrypter côté serveur.

### Implémentation

**Frontend:**
```typescript
// Lire le fichier en base64
const fileBase64 = await fileToBase64(fileBlob);

// Tout en JSON
const payload = {
  amount: 1000,
  currency: 'USD',
  file: {
    name: 'document.pdf',
    contentType: 'application/pdf',
    data: fileBase64  // Base64 du fichier
  }
};

// Encrypter TOUT
const encrypted = await encrypt(payload);

await fetch('/api/disbursements', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ encryptedData: encrypted })
});
```

**Backend:**
```csharp
[HttpPost]
[EncryptedPayload]
public async Task<ActionResult> CreateDisbursement(
    [FromBody] CreateDisbursementDto dto)  // Pas IFormFile!
{
    // dto.File.Data contient le base64
    var fileBytes = Convert.FromBase64String(dto.File.Data);

    // Créer un IFormFile à partir des bytes
    var formFile = new FormFile(
        new MemoryStream(fileBytes),
        0,
        fileBytes.Length,
        dto.File.Name,
        dto.File.Name);

    // Continue normalement...
}
```

### ✅ Avantages

1. **Tout encrypté** - Fichiers + métadonnées protégés
2. **JSON pur** - Pas de multipart
3. **Encryption existante** - Utilise le système actuel
4. **Un seul appel** - Pas de split

### ❌ Inconvénients

1. **Performance terrible** - Base64 augmente taille de 33%
2. **Mémoire** - Tout le fichier en RAM (frontend + backend)
3. **Timeouts** - Gros fichiers = requêtes très longues
4. **Limites serveur** - MaxRequestBodySize dépassée facilement
5. **Breaking change** - Changements majeurs partout
6. **Non scalable** - Ne fonctionne pas pour gros fichiers

### 💡 Use Case

- **Petits fichiers (<1MB)**: Peut-être
- **Gros fichiers**: Absolument pas
- **Production**: Déconseillé

### 🎯 Verdict

**⭐** - Mauvaise solution sauf cas très spécifiques.

---

## 🎯 Solution 5: Attribut [NoEncryption] sur les Endpoints FormData

### Description

Marquer explicitement les endpoints FormData avec `[NoEncryption]` pour les exclure proprement.

### Configuration

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Global"  // Encrypte tout par défaut
  }
}
```

### Backend

```csharp
[HttpPost]
[NoEncryption]  // ✅ Explicite: pas d'encryption ici
public async Task<ActionResult> CreateDisbursement(
    [FromForm] CreateDisbursementCommand command)
{
    // Fonctionne normalement
}

[HttpPut("{id}")]
[NoEncryption]  // ✅ Explicite
public async Task<ActionResult> UpdateDisbursement(
    Guid id,
    [FromForm] EditDisbursementCommand command)
{
    // Fonctionne normalement
}

[HttpGet]  // ❌ Encrypté (pas d'attribut)
public async Task<ActionResult> GetAllDisbursements()
{
    // Encrypté automatiquement
}
```

### Frontend

```typescript
// Aucun changement!
const formData = new FormData();
formData.append('amount', '1000');
formData.append('file', fileBlob);

await fetch('/api/disbursements', {
  method: 'POST',
  body: formData
});
```

### ✅ Avantages

1. **Explicite** - On voit dans le code que c'est non encrypté
2. **Simple** - Juste un attribut à ajouter
3. **Aucun changement frontend** - Fonctionne tel quel
4. **Documenté** - L'attribut documente l'intention
5. **Flexible** - Facile à changer si besoin

### ❌ Inconvénients

1. **Pas d'encryption payload** - Comme Solution 1
2. **Attributs à ajouter** - Sur tous les endpoints FormData
3. **Oublis possibles** - Quelqu'un pourrait oublier l'attribut

### 💡 Use Case

- **Mode Global activé**: Parfait
- **Documentation explicite**: Excellent
- **Team discipline**: Bien

### 🎯 Verdict

**⭐⭐⭐⭐** - Très bonne solution, propre et explicite.

---

## 📊 Comparaison des Solutions

| Solution | Simplicité | Performance | Sécurité | Breaking Change | Verdict |
|----------|-----------|-------------|----------|-----------------|---------|
| 1. NeverEncryptEndpoints | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ | Non | **RECOMMANDÉ** |
| 2. Séparer (2 appels) | ⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ | Oui | Si vraiment nécessaire |
| 3. Hybrid FormData | ⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ | Oui | Trop complexe |
| 4. Base64 | ⭐⭐ | ⭐ | ⭐⭐⭐⭐⭐ | Oui | Non scalable |
| 5. [NoEncryption] | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ | Non | **RECOMMANDÉ** |

---

## 💡 Recommandation Finale

### Pour la majorité des cas: **Solution 1 ou 5**

#### Utiliser Solution 1 (NeverEncryptEndpoints) si:
- Mode Attribute (encryption par opt-in)
- Pas besoin d'encryption globale
- Config centralisée préférée

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Attribute",
    "NeverEncryptEndpoints": ["/api/disbursements"]
  }
}
```

#### Utiliser Solution 5 ([NoEncryption]) si:
- Mode Global (encryption par défaut)
- Documentation explicite dans le code
- Meilleure visibilité pour les devs

```csharp
[HttpPost]
[NoEncryption]
public async Task<ActionResult> CreateDisbursement(...)
```

### Pour compliance stricte: **Solution 2**

Si les métadonnées doivent absolument être encryptées au niveau applicatif:
- Séparer métadonnées (JSON encrypté) et fichiers (multipart)
- 2 appels réseau
- Plus complexe mais plus sécurisé

---

## 🚀 Plan d'Implémentation Recommandé

### Option A: Solution 1 (Config uniquement)

1. Ajouter dans appsettings.json:
```json
"NeverEncryptEndpoints": [
  "/api/disbursements"
]
```

2. ✅ Terminé! Aucun changement de code.

### Option B: Solution 5 (Attributs explicites)

1. Identifier tous les endpoints FormData
2. Ajouter `[NoEncryption]` sur chacun
3. Documenter pourquoi (commentaire)

```csharp
// Pas d'encryption car multipart/form-data avec fichiers binaires
[HttpPost]
[NoEncryption]
public async Task<ActionResult> CreateDisbursement(...)
```

4. ✅ Terminé!

---

## 🔒 Note Sécurité

**Important:** Dans tous les cas, les données sont protégées en transit par HTTPS/TLS.

L'encryption au niveau applicatif ajoute:
- Protection end-to-end
- Protection contre les logs serveur
- Protection contre les intermédiaires (proxies, load balancers)

Mais pour les fichiers binaires, cette encryption applicative est:
- Complexe à implémenter
- Coûteuse en performance
- Souvent non nécessaire si HTTPS strict + sécurité infrastructure

**Verdict:** Pour des fichiers, HTTPS + infrastructure sécurisée est généralement suffisant.

---

**Créé le**: 2025-11-16
**Pour**: DisbursementsController FormData Encryption
