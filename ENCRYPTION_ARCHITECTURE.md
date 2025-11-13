# 🔐 Architecture d'Encryption - Recommandations Sécurité

## ❓ Question Clé: Où Stocker la Clé d'Encryption?

### 🎯 **Réponse Courte**

**NON**, la clé du Key Vault ne doit **PAS** être la même que celle utilisée par le frontend!

---

## 🏗️ **Architecture Recommandée pour AfDB Client Connection**

### **Approche 1: TLS/HTTPS Seulement (Le Plus Simple)** ⭐ **RECOMMANDÉ**

```
┌─────────────────────────────────────────────────────────────┐
│                    CLIENT (Browser)                         │
│                                                             │
│  • Pas de clé stockée                                       │
│  • Pas d'encryption custom                                  │
│  • Tout passe par HTTPS (TLS 1.3)                          │
└─────────────────────────────────────────────────────────────┘
                         │
                         │ HTTPS (TLS 1.3 - AES-256)
                         │ ✅ Certificate pinning
                         │ ✅ Perfect Forward Secrecy
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                   API BACKEND (.NET)                        │
│                                                             │
│  • Reçoit données en clair (protégées par TLS)             │
│  • Encrypte avant stockage DB                              │
│  • Encrypte pour communications externes                    │
│                                                             │
│  Clé: Stockée dans Azure Key Vault                         │
│       (jamais exposée au frontend)                          │
└─────────────────────────────────────────────────────────────┘
                         │
                         │ Encrypted Data
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                  SQL SERVER DATABASE                        │
│                                                             │
│  • Données sensibles encryptées au repos                    │
│  • TDE (Transparent Data Encryption) activé                 │
│  • Column-level encryption pour données critiques           │
└─────────────────────────────────────────────────────────────┘
```

#### ✅ **Pourquoi c'est suffisant?**

1. **HTTPS/TLS 1.3** fournit déjà:
   - Encryption AES-256 (même niveau que ce qu'on implémenterait)
   - Authentication du serveur
   - Protection contre man-in-the-middle
   - Perfect Forward Secrecy

2. **Aucune clé exposée** au frontend

3. **Conformité** aux standards de l'industrie:
   - OWASP recommande HTTPS comme base
   - PCI-DSS accepte TLS 1.2+
   - ISO 27001 valide cette approche

4. **Plus simple** = moins de risques d'erreurs

#### 📝 **Ce que tu dois faire:**

```csharp
// Dans tes controllers - AUCUN changement!
[HttpPost]
public async Task<IActionResult> Create([FromBody] Request request)
{
    // Les données arrivent en clair (protégées par HTTPS)

    // Encrypte SEULEMENT les champs sensibles avant stockage
    if (!string.IsNullOrEmpty(request.SensitiveData))
    {
        request.SensitiveData = _encryptionService.EncryptForStorage(
            request.SensitiveData
        );
    }

    await _repository.SaveAsync(request);
    return Ok();
}
```

---

### **Approche 2: Encryption End-to-End avec Échange de Clés** (Si vraiment requis)

Si l'équipe sécurité **insiste** sur l'encryption applicative en plus de HTTPS:

```
PHASE 1: AUTHENTIFICATION
═══════════════════════════════════════════════════════════════
┌──────────────┐                              ┌──────────────┐
│   FRONTEND   │──[Azure AD Login]───────────►│   BACKEND    │
│              │◄─[JWT + Session ID]──────────│              │
└──────────────┘                              └──────────────┘


PHASE 2: KEY EXCHANGE (après auth)
═══════════════════════════════════════════════════════════════
┌──────────────┐                              ┌──────────────┐
│   FRONTEND   │                              │   BACKEND    │
│              │                              │              │
│ 1. Génère    │                              │              │
│    paire RSA │                              │              │
│    (2048-bit)│                              │              │
│              │                              │              │
│ 2. Envoie    │──[Public Key]───────────────►│              │
│    pub key   │                              │ 3. Génère    │
│              │                              │    clé AES   │
│              │                              │    session   │
│              │                              │                │
│              │                              │ 4. Encrypte  │
│              │                              │    clé AES   │
│              │                              │    avec RSA  │
│              │                              │    pub       │
│              │                              │                │
│ 5. Décrypte  │◄─[Clé AES encryptée]────────│ 6. Stocke    │
│    avec RSA  │                              │    clé in    │
│    privée    │                              │    Redis     │
│              │                              │    (session) │
│ 6. Stocke    │                              │              │
│    clé AES   │                              │              │
│    en mémoire│                              │              │
└──────────────┘                              └──────────────┘


PHASE 3: COMMUNICATION ENCRYPTÉE
═══════════════════════════════════════════════════════════════
┌──────────────┐                              ┌──────────────┐
│   FRONTEND   │                              │   BACKEND    │
│              │                              │              │
│ • Encrypte   │──[Encrypted + Session ID]───►│ • Récupère   │
│   avec clé   │                              │   clé de     │
│   session AES│                              │   Redis      │
│              │                              │ • Décrypte   │
│              │                              │   avec clé   │
│              │◄─[Encrypted Response]────────│   session    │
│ • Décrypte   │                              │ • Encrypte   │
│   réponse    │                              │   réponse    │
└──────────────┘                              └──────────────┘
```

#### 📝 **Implémentation Backend:**

```csharp
// 1. Endpoint pour l'échange de clés
[HttpPost("api/encryption/exchange-key")]
[Authorize] // Nécessite authentification!
public async Task<IActionResult> ExchangeKey([FromBody] KeyExchangeRequest request)
{
    // 1. Récupère la clé publique du client
    var clientPublicKey = request.PublicKeyPem;

    // 2. Génère une clé de session AES-256
    var sessionKey = RandomNumberGenerator.GetBytes(32);
    var sessionId = Guid.NewGuid().ToString();

    // 3. Encrypte la clé de session avec la clé publique du client
    using var rsa = RSA.Create();
    rsa.ImportFromPem(clientPublicKey);
    var encryptedSessionKey = rsa.Encrypt(sessionKey, RSAEncryptionPadding.OaepSHA256);

    // 4. Stocke la clé de session dans Redis avec expiration
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    await _cache.SetAsync(
        $"session_key:{userId}:{sessionId}",
        sessionKey,
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        }
    );

    // 5. Retourne la clé encryptée + session ID
    return Ok(new
    {
        SessionId = sessionId,
        EncryptedKey = Convert.ToBase64String(encryptedSessionKey),
        ExpiresIn = 3600 // secondes
    });
}

// 2. Middleware pour décrypter avec la clé de session
public class SessionKeyDecryptionMiddleware
{
    public async Task InvokeAsync(
        HttpContext context,
        IDistributedCache cache)
    {
        var sessionId = context.Request.Headers["X-Session-Id"].FirstOrDefault();
        if (string.IsNullOrEmpty(sessionId))
        {
            await _next(context);
            return;
        }

        // Récupère la clé de session depuis Redis
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var sessionKey = await cache.GetAsync($"session_key:{userId}:{sessionId}");

        if (sessionKey == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid or expired session" });
            return;
        }

        // Utilise cette clé pour décrypter
        context.Items["SessionKey"] = sessionKey;

        await _next(context);
    }
}
```

#### 📝 **Implémentation Frontend:**

```typescript
// services/encryptionService.ts
import forge from 'node-forge';

class SessionEncryptionService {
  private sessionKey: Uint8Array | null = null;
  private sessionId: string | null = null;
  private keyPair: forge.pki.rsa.KeyPair | null = null;

  async initialize() {
    // 1. Génère une paire RSA côté client
    this.keyPair = forge.pki.rsa.generateKeyPair({ bits: 2048 });

    // 2. Envoie la clé publique au backend
    const publicKeyPem = forge.pki.publicKeyToPem(this.keyPair.publicKey);

    const response = await axios.post('/api/encryption/exchange-key', {
      publicKeyPem
    });

    // 3. Décrypte la clé de session avec la clé privée
    const encryptedKey = forge.util.decode64(response.data.encryptedKey);
    const decryptedKey = this.keyPair.privateKey.decrypt(
      encryptedKey,
      'RSA-OAEP',
      {
        md: forge.md.sha256.create()
      }
    );

    // 4. Stocke la clé de session
    this.sessionKey = new Uint8Array(forge.util.binary.raw.decode(decryptedKey));
    this.sessionId = response.data.sessionId;

    console.log('✅ Session encryption initialized');
  }

  encrypt(data: any): string {
    if (!this.sessionKey) {
      throw new Error('Session not initialized');
    }

    // Utilise AES-GCM avec la clé de session
    const json = JSON.stringify(data);
    const iv = crypto.getRandomValues(new Uint8Array(12));

    // ... encryption avec Web Crypto API

    return base64encode({ iv, ciphertext, tag });
  }

  getSessionId(): string | null {
    return this.sessionId;
  }
}

export const sessionEncryption = new SessionEncryptionService();

// App initialization
await sessionEncryption.initialize();
```

---

## 🎯 **Ma Recommandation Finale**

### **Pour AfDB Client Connection:**

**Utilise l'Approche 1 (HTTPS seulement)** SAUF si:

1. ✅ L'équipe sécurité a une **exigence formelle** documentée
2. ✅ Vous avez fait une **analyse de risque** qui justifie le coût
3. ✅ Vous avez les **ressources** pour maintenir cette complexité

### **Pourquoi?**

| Critère | HTTPS Seul | Encryption App |
|---------|------------|----------------|
| Sécurité du transit | ✅ Excellent | ✅ Excellent |
| Complexité | ✅ Faible | ❌ Élevée |
| Performance | ✅ Rapide | ⚠️ Plus lent |
| Maintenance | ✅ Simple | ❌ Complexe |
| Points de défaillance | ✅ Peu | ⚠️ Plusieurs |
| Conformité | ✅ Standards | ✅ Standards |
| **Coût total** | **✅ Bas** | **❌ Élevé** |

---

## 📋 **Questions à Poser à l'Équipe Sécurité**

Avant d'implémenter l'encryption applicative:

1. ❓ **Quelle menace spécifique** voulez-vous adresser que HTTPS ne couvre pas?
2. ❓ **Avez-vous des exigences réglementaires** (GDPR, PCI-DSS) qui le demandent?
3. ❓ **Quelles sont les données sensibles** qui nécessitent cette protection supplémentaire?
4. ❓ **Avez-vous évalué le coût** (dev, maintenance, performance) vs le bénéfice?
5. ❓ **Comment gérer la rotation des clés** dans ce scénario?

---

## 🎓 **Conclusion**

**TL;DR:**

- 🚫 **Ne partage JAMAIS** la même clé entre frontend et backend
- ✅ **HTTPS est déjà très sécurisé** pour la plupart des cas
- ⚠️ **Encryption applicative** = complexité++ = risques++
- 🎯 **Si vraiment nécessaire**: Utilise l'échange de clés de session

**Mon conseil:** Commence avec HTTPS et encryption au repos. Ajoute l'encryption applicative **seulement si justifié** par une analyse de risque formelle.
