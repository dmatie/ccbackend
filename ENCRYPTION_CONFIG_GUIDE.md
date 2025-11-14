# 🔐 Guide de Configuration Centralisée de l'Encryption

Guide complet pour configurer l'encryption des payloads via `appsettings.json`.

---

## 📋 Vue d'Ensemble

L'encryption est maintenant **100% configurable** depuis `appsettings.json`. Plus besoin de toucher au code!

### Configuration Centralisée

```json
{
  "Encryption": {
    "Enabled": false,
    "PayloadKey": "",
    "Mode": "Attribute",
    "AlwaysEncryptEndpoints": [],
    "NeverEncryptEndpoints": ["/health", "/swagger"]
  }
}
```

---

## 🎯 Options de Configuration

### 1. `Enabled` (bool)

Active ou désactive l'encryption **globalement**.

```json
{
  "Encryption": {
    "Enabled": false  // ❌ Encryption DÉSACTIVÉE partout
  }
}
```

```json
{
  "Encryption": {
    "Enabled": true   // ✅ Encryption ACTIVÉE (selon Mode)
  }
}
```

**Comportement:**
- `false`: Aucune encryption, même avec `[EncryptedPayload]`
- `true`: Encryption activée selon le `Mode`

---

### 2. `PayloadKey` (string)

Clé AES-256 pour l'encryption (32 bytes en base64).

```json
{
  "Encryption": {
    "PayloadKey": "base64-encoded-256-bit-key"
  }
}
```

**Générer une clé:**

```bash
# PowerShell
[Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))

# C#
Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))

# Exemple de clé générée
"vKx8T2mZ9wP4qR5sN6jL3hG7fD1aB8cE+Y0oU4iX2kV="
```

**Important:**
- Clé OBLIGATOIRE si `Enabled = true`
- Doit être identique en dev et prod
- Stocker dans **Azure Key Vault** en production
- Stocker dans **User Secrets** en dev

---

### 3. `Mode` (enum)

Mode de fonctionnement de l'encryption.

#### Mode: `Attribute` (Par défaut)

Encryption activée **uniquement** sur les endpoints avec `[EncryptedPayload]`.

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Attribute"
  }
}
```

**Comportement:**
- Encryption sur endpoints avec `[EncryptedPayload]` ✅
- Pas d'encryption sur les autres endpoints ❌

**Exemple:**

```csharp
[HttpPost]
[EncryptedPayload]  // ✅ Sera encrypté
public async Task<IActionResult> CreateDisbursement(...)

[HttpGet]           // ❌ Ne sera PAS encrypté
public async Task<IActionResult> GetCountries()
```

#### Mode: `Global`

Encryption activée sur **TOUS** les endpoints (sauf `NeverEncryptEndpoints`).

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Global"
  }
}
```

**Comportement:**
- Encryption sur TOUS les endpoints ✅
- Sauf ceux dans `NeverEncryptEndpoints` ❌

**Use Case:**
- Production avec sécurité maximale
- Compliance stricte
- Données ultra-sensibles

---

### 4. `AlwaysEncryptEndpoints` (array)

Liste d'endpoints qui doivent **TOUJOURS** être encryptés, même si `Enabled = false`.

```json
{
  "Encryption": {
    "Enabled": false,
    "AlwaysEncryptEndpoints": [
      "/api/disbursements",
      "/api/users/create"
    ]
  }
}
```

**Use Case:**
- Encryption désactivée globalement
- Mais certains endpoints sensibles doivent être protégés

**Matching:**
- Prefix matching: `/api/disbursements` match `/api/disbursements/123`
- Case insensitive

---

### 5. `NeverEncryptEndpoints` (array)

Liste d'endpoints qui ne doivent **JAMAIS** être encryptés, même si `Enabled = true` en mode `Global`.

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Global",
    "NeverEncryptEndpoints": [
      "/health",
      "/swagger",
      "/_blazor",
      "/api/references"
    ]
  }
}
```

**Use Case:**
- Endpoints publics (health checks, swagger)
- Endpoints de référence non sensibles
- Intégrations tierces qui ne supportent pas l'encryption

**Par défaut inclus:**
- `/health`
- `/swagger`
- `/_blazor`

---

## 📊 Exemples de Configuration

### Exemple 1: Développement Local (Pas d'encryption)

```json
{
  "Encryption": {
    "Enabled": false,
    "PayloadKey": "",
    "Mode": "Attribute",
    "AlwaysEncryptEndpoints": [],
    "NeverEncryptEndpoints": ["/health", "/swagger"]
  }
}
```

**Résultat:**
- ❌ Aucune encryption
- Plus simple pour débugger
- Pas besoin de clé

---

### Exemple 2: Production (Mode Attribute)

```json
{
  "Encryption": {
    "Enabled": true,
    "PayloadKey": "vKx8T2mZ9wP4qR5sN6jL3hG7fD1aB8cE+Y0oU4iX2kV=",
    "Mode": "Attribute",
    "AlwaysEncryptEndpoints": [],
    "NeverEncryptEndpoints": ["/health", "/swagger", "/api/references"]
  }
}
```

**Résultat:**
- ✅ Encryption sur endpoints avec `[EncryptedPayload]`
- ❌ Pas d'encryption sur `/api/references`
- Contrôle granulaire par attribut

---

### Exemple 3: Production (Mode Global - Maximum Sécurité)

```json
{
  "Encryption": {
    "Enabled": true,
    "PayloadKey": "vKx8T2mZ9wP4qR5sN6jL3hG7fD1aB8cE+Y0oU4iX2kV=",
    "Mode": "Global",
    "AlwaysEncryptEndpoints": [],
    "NeverEncryptEndpoints": ["/health", "/api/references/countries"]
  }
}
```

**Résultat:**
- ✅ TOUS les endpoints encryptés
- ❌ Sauf `/health` et `/api/references/countries`
- Sécurité maximale

---

### Exemple 4: Hybride (Encryption désactivée sauf endpoints critiques)

```json
{
  "Encryption": {
    "Enabled": false,
    "PayloadKey": "vKx8T2mZ9wP4qR5sN6jL3hG7fD1aB8cE+Y0oU4iX2kV=",
    "Mode": "Attribute",
    "AlwaysEncryptEndpoints": [
      "/api/disbursements",
      "/api/users/create",
      "/api/accessrequests"
    ],
    "NeverEncryptEndpoints": []
  }
}
```

**Résultat:**
- ❌ Encryption désactivée globalement
- ✅ SAUF pour `/api/disbursements`, `/api/users/create`, `/api/accessrequests`
- Bon compromis performance/sécurité

---

## 🔧 Gestion des Clés

### Développement Local

**Option 1: User Secrets (Recommandé)**

```bash
cd src/Afdb.ClientConnection.Api

# Générer et stocker la clé
dotnet user-secrets set "Encryption:PayloadKey" "vKx8T2mZ9wP4qR5sN6jL3hG7fD1aB8cE+Y0oU4iX2kV="
```

**Option 2: Variables d'environnement**

```bash
# Linux/Mac
export Encryption__PayloadKey="vKx8T2mZ9wP4qR5sN6jL3hG7fD1aB8cE+Y0oU4iX2kV="

# Windows
set Encryption__PayloadKey=vKx8T2mZ9wP4qR5sN6jL3hG7fD1aB8cE+Y0oU4iX2kV=
```

### Production

**Azure Key Vault (OBLIGATOIRE)**

1. **Créer le secret dans Key Vault:**

```bash
az keyvault secret set \
  --vault-name your-keyvault \
  --name PayloadEncryptionKey \
  --value "vKx8T2mZ9wP4qR5sN6jL3hG7fD1aB8cE+Y0oU4iX2kV="
```

2. **Référencer dans appsettings.Production.json:**

```json
{
  "KeyVault": {
    "VaultUri": "https://your-keyvault.vault.azure.net/",
    "PayloadKeyName": "PayloadEncryptionKey"
  },
  "Encryption": {
    "Enabled": true,
    "PayloadKey": "",  // Sera chargé depuis Key Vault
    "Mode": "Global"
  }
}
```

3. **Charger depuis Key Vault dans Program.cs** (déjà fait):

```csharp
var keyVaultUrl = builder.Configuration["KeyVault:VaultUri"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new DefaultAzureCredential());
}
```

---

## 🎮 Utilisation dans le Code

### Attributs

Tu peux toujours utiliser les attributs pour contrôler finement:

```csharp
// Force l'encryption (même si Enabled = false en mode Attribute)
[HttpPost]
[EncryptedPayload]
public async Task<IActionResult> CreateDisbursement(...)

// Empêche l'encryption (même si Enabled = true en mode Global)
[HttpGet]
[NoEncryption]
public async Task<IActionResult> GetPublicData()
```

### Check programmatique

```csharp
public class MyController : ControllerBase
{
    private readonly IPayloadEncryptionService _encryptionService;

    public MyController(IPayloadEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    [HttpGet]
    public IActionResult GetStatus()
    {
        var isEnabled = _encryptionService.IsEnabled;
        var shouldEncrypt = _encryptionService.ShouldEncrypt("/api/mypath");

        return Ok(new { isEnabled, shouldEncrypt });
    }
}
```

---

## 🔍 Logique de Décision

### Pour l'Encryption (Réponses Sortantes)

Voici comment l'encryption des réponses est déterminée:

```
1. Encryption globalement désactivée (Enabled = false)?
   └─ OUI → Vérifie AlwaysEncryptEndpoints
      ├─ Endpoint dans AlwaysEncryptEndpoints? → ✅ ENCRYPTS
      └─ Sinon → ❌ PAS D'ENCRYPTION
   └─ NON → Continue

2. Attribut [NoEncryption] présent?
   └─ OUI → ❌ PAS D'ENCRYPTION
   └─ NON → Continue

3. Endpoint dans NeverEncryptEndpoints?
   └─ OUI → ❌ PAS D'ENCRYPTION
   └─ NON → Continue

4. Endpoint dans AlwaysEncryptEndpoints?
   └─ OUI → ✅ ENCRYPTS
   └─ NON → Continue

5. Mode = Global?
   └─ OUI → ✅ ENCRYPTS
   └─ NON → Continue

6. Attribut [EncryptedPayload] présent?
   └─ OUI → ✅ ENCRYPTS
   └─ NON → ❌ PAS D'ENCRYPTION
```

### Pour le Décryptage (Requêtes Entrantes)

La **même logique** s'applique pour le décryptage des requêtes:

```
1. Encryption globalement désactivée (Enabled = false)?
   └─ OUI → Vérifie AlwaysEncryptEndpoints
      ├─ Endpoint dans AlwaysEncryptEndpoints? → ✅ DÉCRYPTE
      └─ Sinon → ❌ PAS DE DÉCRYPTAGE
   └─ NON → Continue

2. Attribut [NoEncryption] présent?
   └─ OUI → ❌ PAS DE DÉCRYPTAGE
   └─ NON → Continue

3. Endpoint dans NeverEncryptEndpoints?
   └─ OUI → ❌ PAS DE DÉCRYPTAGE
   └─ NON → Continue

4. Endpoint dans AlwaysEncryptEndpoints?
   └─ OUI → ✅ DÉCRYPTE
   └─ NON → Continue

5. Mode = Global?
   └─ OUI → ✅ DÉCRYPTE
   └─ NON → Continue

6. Attribut [EncryptedPayload] présent avec EncryptRequest = true?
   └─ OUI → ✅ DÉCRYPTE
   └─ NON → ❌ PAS DE DÉCRYPTAGE
```

**Important:** Les deux middlewares (encryption et décryptage) utilisent la **même configuration**. Si un endpoint est configuré pour être encrypté, il sera automatiquement décrypté aussi!

### Comportement Symétrique

L'encryption est **bidirectionnelle et symétrique**:

| Configuration | Requête (Client → API) | Réponse (API → Client) |
|---------------|------------------------|------------------------|
| Enabled: false | Pas de décryptage ❌ | Pas d'encryption ❌ |
| Enabled: true, Mode: Global | Décrypte tout ✅ | Encrypte tout ✅ |
| Enabled: true, Mode: Attribute | Décrypte si [EncryptedPayload] ✅ | Encrypte si [EncryptedPayload] ✅ |
| AlwaysEncryptEndpoints: ["/api/x"] | Décrypte /api/x ✅ | Encrypte /api/x ✅ |
| NeverEncryptEndpoints: ["/api/y"] | Ne décrypte pas /api/y ❌ | N'encrypte pas /api/y ❌ |

**Exemple pratique:**

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Global",
    "NeverEncryptEndpoints": ["/health", "/api/references"]
  }
}
```

**Résultat:**
- POST /api/disbursements → **Décrypte requête ✅**, **Encrypte réponse ✅**
- GET /api/references/countries → **Ne décrypte pas ❌**, **N'encrypte pas ❌**
- GET /health → **Ne décrypte pas ❌**, **N'encrypte pas ❌**

---

## 📊 Matrice de Décision

| Enabled | Mode      | [EncryptedPayload] | AlwaysEncrypt | NeverEncrypt | Résultat |
|---------|-----------|-------------------|---------------|--------------|----------|
| false   | -         | -                 | No            | -            | ❌       |
| false   | -         | -                 | Yes           | -            | ✅       |
| true    | Attribute | No                | No            | No           | ❌       |
| true    | Attribute | Yes               | No            | No           | ✅       |
| true    | Attribute | Yes               | No            | Yes          | ❌       |
| true    | Global    | -                 | No            | No           | ✅       |
| true    | Global    | -                 | No            | Yes          | ❌       |

---

## 🚀 Migration depuis l'Ancien Système

Si tu utilisais l'encryption avant:

**Avant (uniquement par attribut):**

```csharp
[HttpPost]
[EncryptedPayload]  // Nécessaire
public async Task<IActionResult> Create(...)
```

**Après (configuration centralisée):**

```json
{
  "Encryption": {
    "Enabled": true,
    "Mode": "Global"  // Tous les endpoints encryptés
  }
}
```

```csharp
[HttpPost]  // Plus besoin d'attribut si Mode = Global!
public async Task<IActionResult> Create(...)
```

---

## 🧪 Testing

### Tester la Configuration

```bash
# Health check (ne doit jamais être encrypté)
curl http://localhost:5000/health

# Endpoint encrypté
curl http://localhost:5000/api/disbursements
# Réponse: {"encryptedData": "base64..."}
```

### Logs

L'encryption log son état au démarrage:

```
info: PayloadEncryptionService initialized - Enabled: True, Mode: Global
```

Si désactivée:

```
info: Payload encryption is DISABLED globally
```

---

## ⚠️ Bonnes Pratiques

### ✅ À FAIRE

1. **User Secrets en dev:**
   ```bash
   dotnet user-secrets set "Encryption:PayloadKey" "..."
   ```

2. **Azure Key Vault en prod:**
   - Clé stockée dans Key Vault
   - Managed Identity pour accès

3. **Mode Attribute en dev:**
   - Plus simple à débugger
   - Contrôle granulaire

4. **Mode Global en prod:**
   - Sécurité maximale
   - Compliance

5. **NeverEncryptEndpoints pour health checks:**
   - Monitoring externe ne supporte pas encryption

### ❌ À ÉVITER

1. **Clé hardcodée dans appsettings.json:**
   ```json
   "PayloadKey": "vKx8..." // ❌ DANGER!
   ```

2. **Même clé en dev et prod:**
   - Utilise des clés différentes

3. **Encryption sur tous les endpoints sans raison:**
   - Impact performance
   - Complexité inutile

4. **Oublier NeverEncryptEndpoints:**
   - Health checks ne fonctionneront plus

---

## 📚 Références

- **EncryptionSettings.cs**: `/src/Afdb.ClientConnection.Infrastructure/Settings/`
- **PayloadEncryptionService.cs**: `/src/Afdb.ClientConnection.Infrastructure/Services/`
- **PayloadEncryptionMiddleware.cs**: `/src/Afdb.ClientConnection.Api/Middleware/`

---

**Créé le**: 2025-11-13
**Version**: 2.0 (Configuration Centralisée)
