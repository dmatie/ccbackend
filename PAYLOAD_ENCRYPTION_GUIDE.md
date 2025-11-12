# 🔐 Guide d'Implémentation de l'Encryption des Payloads

## 📋 Vue d'Ensemble

Ce système permet d'encrypter automatiquement les payloads entre le frontend et le backend **sans modifier les signatures des endpoints existants**.

### ✨ Caractéristiques

- ✅ **AES-256-GCM**: Encryption moderne et sécurisée
- ✅ **Transparent**: Aucune modification des controllers
- ✅ **Sélectif**: Activation par endpoint via attributs
- ✅ **Bidirectionnel**: Encrypts requêtes ET réponses
- ✅ **Authentifié**: Protection contre la manipulation des données (GCM)

---

## 🏗️ Architecture

```
┌─────────────┐                                    ┌─────────────┐
│   CLIENT    │                                    │   SERVER    │
│  (React)    │                                    │  (ASP.NET)  │
└─────────────┘                                    └─────────────┘
      │                                                    │
      │ 1. Encrypt payload avec clé partagée              │
      │ { "encryptedData": "base64..." }                  │
      ├───────────────────────────────────────────────────>│
      │                                                    │
      │           2. PayloadDecryptionMiddleware          │
      │              - Décode base64                      │
      │              - Decrypt AES-GCM                    │
      │              - Remplace request body              │
      │                                                    │
      │                    3. Controller                  │
      │                 (Reçoit JSON plain)               │
      │                                                    │
      │           4. PayloadEncryptionMiddleware          │
      │              - Capture response                   │
      │              - Encrypt AES-GCM                    │
      │              - Encode base64                      │
      │                                                    │
      │<───────────────────────────────────────────────────│
      │ { "encryptedData": "base64..." }                  │
      │                                                    │
      │ 5. Decrypt response avec clé partagée             │
      │                                                    │
```

---

## ⚙️ Configuration Initiale

### 1. Générer une Clé d'Encryption

**Option A: Via C# (Recommandé)**

```csharp
using System.Security.Cryptography;

var key = RandomNumberGenerator.GetBytes(32); // 256 bits
var keyBase64 = Convert.ToBase64String(key);
Console.WriteLine(keyBase64);
```

**Option B: Via Helper intégré**

Ajoutez temporairement ce endpoint dans un controller:

```csharp
[HttpGet("generate-encryption-key")]
public IActionResult GenerateKey()
{
    Afdb.ClientConnection.Api.Helpers.EncryptionKeyGenerator.PrintNewKey();
    return Ok("Check console output");
}
```

**Option C: Via PowerShell**

```powershell
$bytes = New-Object byte[] 32
[System.Security.Cryptography.RNGCryptoServiceProvider]::Create().GetBytes($bytes)
[Convert]::ToBase64String($bytes)
```

### 2. Stocker la Clé de Manière Sécurisée

**❌ NE JAMAIS faire (production):**
```json
// appsettings.json
{
  "Encryption": {
    "PayloadKey": "votre-cle-ici" // ❌ Mauvaise pratique!
  }
}
```

**✅ DÉVELOPPEMENT: User Secrets**

```bash
dotnet user-secrets set "Encryption:PayloadKey" "VOTRE_CLE_BASE64_ICI"
```

**✅ PRODUCTION: Azure Key Vault**

1. Créez un secret dans Azure Key Vault:
   - Nom: `EncryptionPayloadKey`
   - Valeur: Votre clé base64

2. Dans `appsettings.Production.json`:
```json
{
  "KeyVault": {
    "VaultUri": "https://votre-keyvault.vault.azure.net/"
  }
}
```

La clé sera automatiquement récupérée depuis Key Vault au démarrage.

**✅ VARIABLES D'ENVIRONNEMENT**

```bash
export Encryption__PayloadKey="VOTRE_CLE_BASE64_ICI"
```

---

## 🎯 Utilisation dans les Controllers

### Option 1: Activation par Endpoint (Recommandé)

```csharp
using Afdb.ClientConnection.Api.Attributes;

[ApiController]
[Route("api/[controller]")]
public class SensitiveDataController : ControllerBase
{
    // ✅ Cet endpoint utilise l'encryption
    [HttpPost("create")]
    [EncryptedPayload] // 👈 Active l'encryption requête ET réponse
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        // Votre code existant - AUCUNE MODIFICATION nécessaire!
        var result = await _service.CreateAsync(request);
        return Ok(result);
    }

    // ✅ Encryption seulement de la réponse
    [HttpGet("{id}")]
    [EncryptedPayload(encryptRequest: false, encryptResponse: true)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetAsync(id);
        return Ok(result);
    }

    // ❌ Cet endpoint n'utilise PAS l'encryption
    [HttpGet("public")]
    public async Task<IActionResult> GetPublic()
    {
        return Ok("Public data");
    }
}
```

### Option 2: Activation au niveau du Controller

```csharp
[ApiController]
[Route("api/[controller]")]
[EncryptedPayload] // 👈 Tous les endpoints de ce controller sont encryptés
public class SecureController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Request request)
    {
        // Automatiquement encrypté
        return Ok(response);
    }

    [HttpGet]
    [NoEncryption] // 👈 Exception: cet endpoint n'est pas encrypté
    public async Task<IActionResult> GetPublic()
    {
        return Ok("Public");
    }
}
```

---

## 💻 Intégration Frontend

### 1. Service d'Encryption (React/TypeScript)

```typescript
// services/encryptionService.ts
import CryptoJS from 'crypto-js';

class PayloadEncryptionService {
  private key: string;

  constructor() {
    // ⚠️ En production, récupérez la clé via un mécanisme sécurisé
    // (ex: après authentification, le backend envoie la clé encryptée avec la clé publique du client)
    this.key = import.meta.env.VITE_ENCRYPTION_KEY;

    if (!this.key) {
      throw new Error('Encryption key not configured');
    }
  }

  /**
   * Encrypts un payload pour l'envoyer au backend
   */
  encrypt<T>(payload: T): { encryptedData: string } {
    const json = JSON.stringify(payload);
    const encrypted = CryptoJS.AES.encrypt(json, this.key).toString();
    return { encryptedData: encrypted };
  }

  /**
   * Décrypte une réponse du backend
   */
  decrypt<T>(encryptedPayload: { encryptedData: string }): T {
    const decrypted = CryptoJS.AES.decrypt(
      encryptedPayload.encryptedData,
      this.key
    );
    const json = decrypted.toString(CryptoJS.enc.Utf8);
    return JSON.parse(json);
  }
}

export const encryptionService = new PayloadEncryptionService();
```

### 2. Axios Interceptor (Automatique)

```typescript
// api/axiosConfig.ts
import axios from 'axios';
import { encryptionService } from '../services/encryptionService';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

// Intercepteur pour encrypter les requêtes sortantes
api.interceptors.request.use(
  (config) => {
    // Vérifie si l'endpoint nécessite l'encryption
    // (vous pouvez ajouter un header custom pour l'indiquer)
    const needsEncryption = config.headers['X-Encrypt-Payload'] === 'true';

    if (needsEncryption && config.data) {
      config.data = encryptionService.encrypt(config.data);
    }

    return config;
  },
  (error) => Promise.reject(error)
);

// Intercepteur pour décrypter les réponses entrantes
api.interceptors.response.use(
  (response) => {
    // Vérifie si la réponse est encryptée
    if (response.data && response.data.encryptedData) {
      response.data = encryptionService.decrypt(response.data);
    }

    return response;
  },
  (error) => Promise.reject(error)
);

export default api;
```

### 3. Utilisation dans les Components

```typescript
// components/CreateForm.tsx
import api from '../api/axiosConfig';

const CreateForm = () => {
  const handleSubmit = async (formData) => {
    try {
      const response = await api.post('/api/sensitive/create', formData, {
        headers: {
          'X-Encrypt-Payload': 'true', // 👈 Active l'encryption pour cette requête
        },
      });

      console.log('Response:', response.data); // Déjà décrypté!
    } catch (error) {
      console.error('Error:', error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {/* Votre formulaire */}
    </form>
  );
};
```

---

## 🔍 Tests

### Test Unitaire du Service

```csharp
[Fact]
public void Encrypt_Decrypt_ShouldReturnOriginalPayload()
{
    // Arrange
    var config = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string>
        {
            ["Encryption:PayloadKey"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
        })
        .Build();

    var logger = new Mock<ILogger<PayloadEncryptionService>>();
    var service = new PayloadEncryptionService(config, logger.Object);

    var original = new { Name = "Test", Value = 123 };

    // Act
    var encrypted = service.Encrypt(original);
    var decrypted = service.Decrypt<dynamic>(encrypted);

    // Assert
    Assert.Equal(original.Name, decrypted.Name);
    Assert.Equal(original.Value, decrypted.Value);
}
```

### Test d'Intégration

```csharp
[Fact]
public async Task EncryptedEndpoint_ShouldWorkTransparently()
{
    // Arrange
    var request = new CreateRequest { Name = "Test" };
    var encryptedPayload = _encryptionService.Encrypt(request);

    // Act
    var response = await _client.PostAsJsonAsync("/api/sensitive/create", new
    {
        encryptedData = encryptedPayload
    });

    // Assert
    response.EnsureSuccessStatusCode();
    var encryptedResponse = await response.Content.ReadFromJsonAsync<EncryptedWrapper>();
    var decrypted = _encryptionService.Decrypt<CreateResponse>(encryptedResponse.EncryptedData);

    Assert.NotNull(decrypted);
}
```

---

## 📊 Monitoring et Logs

Les middlewares loggent automatiquement:

```
[Information] PayloadEncryptionService initialized with AES-256-GCM
[Debug] Successfully decrypted payload for /api/sensitive/create (size: 245 bytes)
[Debug] Successfully encrypted response for /api/sensitive/create (original: 189 bytes, encrypted: 312 bytes)
[Error] Failed to decrypt payload for /api/sensitive/create - authentication failed
```

---

## ⚠️ Considérations de Sécurité

### ✅ Bonnes Pratiques

1. **Rotation des Clés**: Changez la clé périodiquement
2. **Clés Différentes**: Utilisez des clés différentes par environnement
3. **Logs**: Ne jamais logger les payloads décryptés
4. **HTTPS**: L'encryption des payloads ne remplace PAS HTTPS
5. **Key Management**: Utilisez Azure Key Vault en production

### ❌ À Éviter

1. ❌ Stocker la clé dans le code source
2. ❌ Utiliser la même clé en dev et prod
3. ❌ Désactiver la validation du certificat HTTPS
4. ❌ Logger les données sensibles
5. ❌ Partager la clé par email/Slack

---

## 🚀 Déploiement

### Checklist Avant Production

- [ ] Clé générée de manière sécurisée (256 bits)
- [ ] Clé stockée dans Azure Key Vault
- [ ] Tests d'intégration passent
- [ ] Logs configurés correctement
- [ ] Monitoring en place
- [ ] Documentation à jour
- [ ] Frontend configuré avec la clé
- [ ] Rollback plan prêt

---

## 🐛 Dépannage

### Problème: "Encryption key must be 256 bits"

**Cause**: La clé n'est pas de la bonne taille

**Solution**: Générez une nouvelle clé de 32 bytes (256 bits)

```csharp
var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
```

### Problème: "Failed to decrypt payload - authentication failed"

**Cause**:
- La clé utilisée côté client est différente de celle côté serveur
- Le payload a été modifié en transit
- Format invalide

**Solution**: Vérifiez que les deux côtés utilisent la même clé

### Problème: Performance Impact

**Cause**: L'encryption/decryption prend du temps

**Solutions**:
- Utilisez l'encryption seulement pour les endpoints sensibles
- Augmentez les ressources du serveur si nécessaire
- Considérez la mise en cache côté client

---

## 📚 Ressources

- [AES-GCM Specification](https://csrc.nist.gov/publications/detail/sp/800-38d/final)
- [Azure Key Vault Best Practices](https://docs.microsoft.com/en-us/azure/key-vault/general/best-practices)
- [OWASP Cryptographic Storage Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Cryptographic_Storage_Cheat_Sheet.html)

---

## 📞 Support

Pour toute question ou problème:
1. Consultez cette documentation
2. Vérifiez les logs de l'application
3. Contactez l'équipe de sécurité
