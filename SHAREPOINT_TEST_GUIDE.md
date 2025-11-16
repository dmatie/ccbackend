# 🧪 Guide de Test - SharePoint Service

Guide complet pour tester les fonctionnalités d'upload et download SharePoint.

---

## 📋 Controller: SharePointTestController

**Fichier créé:** `src/Afdb.ClientConnection.Api/Controllers/SharePointTestController.cs`

**Base URL:** `/api/sharepointtest`

**Authentification:** Tous les endpoints nécessitent un token valide (`[Authorize]`)

---

## 🎯 Endpoints Disponibles

### 1. GET `/api/sharepointtest/config` - Vérifier la Configuration

Vérifie l'état de la configuration SharePoint.

#### Requête:

```bash
GET /api/sharepointtest/config
Authorization: Bearer {votre-token}
```

#### Réponse:

```json
{
  "useSharePointStorage": true,
  "siteIdConfigured": true,
  "driveIdConfigured": true,
  "siteId": "votre-site-id",
  "driveId": "votre-drive-id",
  "isReady": true
}
```

#### Avec curl:

```bash
curl -X GET "https://localhost:5001/api/sharepointtest/config" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Avec Swagger:

```
GET /api/sharepointtest/config
```

---

### 2. POST `/api/sharepointtest/upload` - Tester l'Upload

Teste l'upload d'un fichier vers SharePoint.

#### Paramètres:

| Paramètre | Type | Requis | Description |
|-----------|------|--------|-------------|
| `file` | IFormFile | ✅ Oui | Fichier à uploader |
| `folderPath` | string | ❌ Non | Chemin du dossier (défaut: "test") |

#### Requête:

```bash
POST /api/sharepointtest/upload
Authorization: Bearer {votre-token}
Content-Type: multipart/form-data

file: <fichier>
folderPath: "test/documents"
```

#### Réponse Succès (200):

```json
{
  "success": true,
  "message": "Fichier uploadé avec succès",
  "fileName": "document.pdf",
  "fileSize": 1024567,
  "folderPath": "test/documents",
  "webUrl": "https://yourtenant.sharepoint.com/sites/yoursite/Shared Documents/test/documents/document.pdf",
  "uploadedAt": "2025-11-16T10:30:00Z"
}
```

#### Réponse Erreur (400/500):

```json
{
  "success": false,
  "message": "Erreur lors de l'upload",
  "error": "Description de l'erreur",
  "details": "Détails supplémentaires"
}
```

#### Avec curl:

```bash
curl -X POST "https://localhost:5001/api/sharepointtest/upload" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@/path/to/document.pdf" \
  -F "folderPath=test/documents"
```

#### Avec PowerShell:

```powershell
$token = "YOUR_TOKEN"
$headers = @{
    "Authorization" = "Bearer $token"
}

$form = @{
    file = Get-Item -Path "C:\path\to\document.pdf"
    folderPath = "test/documents"
}

Invoke-RestMethod -Uri "https://localhost:5001/api/sharepointtest/upload" `
    -Method Post `
    -Headers $headers `
    -Form $form
```

#### Avec Postman:

1. Méthode: `POST`
2. URL: `https://localhost:5001/api/sharepointtest/upload`
3. Headers:
   - `Authorization`: `Bearer YOUR_TOKEN`
4. Body:
   - Type: `form-data`
   - Clé: `file` | Type: `File` | Valeur: Sélectionner un fichier
   - Clé: `folderPath` | Type: `Text` | Valeur: `test/documents`

---

### 3. GET `/api/sharepointtest/download` - Tester le Download

Teste le téléchargement d'un fichier depuis SharePoint via son WebUrl.

#### Paramètres:

| Paramètre | Type | Requis | Description |
|-----------|------|--------|-------------|
| `webUrl` | string (query) | ✅ Oui | URL web complète du fichier SharePoint |

#### Requête:

```bash
GET /api/sharepointtest/download?webUrl={url-encodée}
Authorization: Bearer {votre-token}
```

#### Réponse Succès (200):

```
Content-Type: application/pdf (ou autre selon le fichier)
Content-Disposition: attachment; filename="document.pdf"

<contenu binaire du fichier>
```

#### Réponse Erreur (404):

```json
{
  "success": false,
  "message": "Fichier non trouvé",
  "webUrl": "https://...",
  "error": "Description de l'erreur"
}
```

#### Avec curl:

```bash
# Encoder l'URL
WEB_URL="https://yourtenant.sharepoint.com/sites/yoursite/Shared Documents/test/document.pdf"
ENCODED_URL=$(echo $WEB_URL | jq -sRr @uri)

curl -X GET "https://localhost:5001/api/sharepointtest/download?webUrl=$ENCODED_URL" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  --output downloaded-file.pdf
```

#### Avec PowerShell:

```powershell
$token = "YOUR_TOKEN"
$webUrl = "https://yourtenant.sharepoint.com/sites/yoursite/Shared Documents/test/document.pdf"
$encodedUrl = [System.Web.HttpUtility]::UrlEncode($webUrl)

$headers = @{
    "Authorization" = "Bearer $token"
}

Invoke-RestMethod -Uri "https://localhost:5001/api/sharepointtest/download?webUrl=$encodedUrl" `
    -Method Get `
    -Headers $headers `
    -OutFile "downloaded-file.pdf"
```

#### Avec Postman:

1. Méthode: `GET`
2. URL: `https://localhost:5001/api/sharepointtest/download`
3. Headers:
   - `Authorization`: `Bearer YOUR_TOKEN`
4. Params:
   - Clé: `webUrl`
   - Valeur: `https://yourtenant.sharepoint.com/.../document.pdf`
5. Send and Save Response

---

### 4. POST `/api/sharepointtest/full-cycle` - Test Cycle Complet

Teste le cycle complet: upload puis download immédiat avec vérification d'intégrité.

#### Paramètres:

| Paramètre | Type | Requis | Description |
|-----------|------|--------|-------------|
| `file` | IFormFile | ✅ Oui | Fichier à tester |
| `folderPath` | string | ❌ Non | Chemin du dossier (défaut: "test/cycle") |

#### Requête:

```bash
POST /api/sharepointtest/full-cycle
Authorization: Bearer {votre-token}
Content-Type: multipart/form-data

file: <fichier>
folderPath: "test/cycle"
```

#### Réponse Succès (200):

```json
{
  "success": true,
  "message": "Test cycle complet réussi - Intégrité du fichier vérifiée",
  "fileName": "document.pdf",
  "originalSize": 1024567,
  "downloadedSize": 1024567,
  "integrityCheck": true,
  "folderPath": "test/cycle",
  "webUrl": "https://...",
  "contentType": "application/pdf",
  "uploadDurationMs": 1234,
  "downloadDurationMs": 567,
  "totalDurationMs": 1801,
  "testedAt": "2025-11-16T10:30:00Z"
}
```

#### Avec curl:

```bash
curl -X POST "https://localhost:5001/api/sharepointtest/full-cycle" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@/path/to/document.pdf" \
  -F "folderPath=test/cycle"
```

#### Avec PowerShell:

```powershell
$token = "YOUR_TOKEN"
$headers = @{
    "Authorization" = "Bearer $token"
}

$form = @{
    file = Get-Item -Path "C:\path\to\document.pdf"
    folderPath = "test/cycle"
}

$response = Invoke-RestMethod -Uri "https://localhost:5001/api/sharepointtest/full-cycle" `
    -Method Post `
    -Headers $headers `
    -Form $form

Write-Host "Intégrité: $($response.integrityCheck)"
Write-Host "Upload: $($response.uploadDurationMs)ms"
Write-Host "Download: $($response.downloadDurationMs)ms"
Write-Host "Total: $($response.totalDurationMs)ms"
```

---

## 🔧 Configuration Requise

### appsettings.json

```json
{
  "SharePoint": {
    "SiteId": "votre-site-id-guid",
    "DriveId": "votre-drive-id-guid",
    "UseSharePointStorage": true
  }
}
```

### Comment obtenir les IDs SharePoint

#### Méthode 1: Via Graph Explorer

1. Aller sur https://developer.microsoft.com/graph/graph-explorer
2. Se connecter avec un compte ayant accès au site

**Pour obtenir le SiteId:**
```
GET https://graph.microsoft.com/v1.0/sites?search=nom-de-votre-site
```

Ou si vous connaissez le hostname et path:
```
GET https://graph.microsoft.com/v1.0/sites/{hostname}:/{site-path}
```

Exemple:
```
GET https://graph.microsoft.com/v1.0/sites/contoso.sharepoint.com:/sites/TeamSite
```

**Pour obtenir le DriveId:**
```
GET https://graph.microsoft.com/v1.0/sites/{siteId}/drives
```

#### Méthode 2: Via PowerShell

```powershell
# Installer le module si nécessaire
Install-Module -Name Microsoft.Graph -Scope CurrentUser

# Se connecter
Connect-MgGraph -Scopes "Sites.Read.All"

# Obtenir le site
$site = Get-MgSite -Search "nom-de-votre-site"
$siteId = $site.Id
Write-Host "SiteId: $siteId"

# Obtenir les drives du site
$drives = Get-MgSiteDrive -SiteId $siteId
$driveId = $drives[0].Id
Write-Host "DriveId: $driveId"
```

---

## 📝 Scénarios de Test

### Scénario 1: Vérification de Base

```bash
# 1. Vérifier la configuration
curl -X GET "https://localhost:5001/api/sharepointtest/config" \
  -H "Authorization: Bearer YOUR_TOKEN"

# Vérifier que isReady = true
```

### Scénario 2: Test Upload Simple

```bash
# 2. Uploader un fichier de test
curl -X POST "https://localhost:5001/api/sharepointtest/upload" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@test.pdf" \
  -F "folderPath=test"

# Récupérer le webUrl dans la réponse
```

### Scénario 3: Test Download

```bash
# 3. Télécharger le fichier uploadé
WEB_URL="<webUrl de l'étape 2>"
ENCODED_URL=$(echo $WEB_URL | jq -sRr @uri)

curl -X GET "https://localhost:5001/api/sharepointtest/download?webUrl=$ENCODED_URL" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  --output downloaded.pdf

# Comparer avec le fichier original
diff test.pdf downloaded.pdf
```

### Scénario 4: Test Cycle Complet

```bash
# 4. Test complet avec vérification d'intégrité
curl -X POST "https://localhost:5001/api/sharepointtest/full-cycle" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@test.pdf" \
  -F "folderPath=test/cycle" | jq '.'

# Vérifier que integrityCheck = true
```

---

## 🧪 Tests avec Différents Types de Fichiers

### PDF

```bash
curl -X POST "https://localhost:5001/api/sharepointtest/upload" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@document.pdf" \
  -F "folderPath=test/pdf"
```

### Images

```bash
curl -X POST "https://localhost:5001/api/sharepointtest/upload" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@photo.jpg" \
  -F "folderPath=test/images"
```

### Documents Office

```bash
curl -X POST "https://localhost:5001/api/sharepointtest/upload" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@rapport.docx" \
  -F "folderPath=test/office"
```

### Fichiers volumineux

```bash
# Créer un fichier de test de 10MB
dd if=/dev/zero of=big-file.bin bs=1M count=10

# Tester upload + download + intégrité
curl -X POST "https://localhost:5001/api/sharepointtest/full-cycle" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -F "file=@big-file.bin" \
  -F "folderPath=test/large"
```

---

## 🔍 Debugging et Logs

### Activer les logs détaillés

**appsettings.Development.json:**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Afdb.ClientConnection.Api.Controllers.SharePointTestController": "Debug",
      "Afdb.ClientConnection.Infrastructure.Services.SharePointGraphService": "Debug"
    }
  }
}
```

### Logs attendus pour upload:

```
[Information] Test upload: fichier=document.pdf, taille=1024567 bytes, dossier=test/documents
[Debug] Vérification/création du dossier: test/documents
[Debug] Upload du fichier vers SharePoint...
[Debug] Mise à jour des métadonnées...
[Information] Upload réussi: https://...
```

### Logs attendus pour download:

```
[Information] Test download: webUrl=https://...
[Debug] Résolution de l'élément à partir de l'URL...
[Debug] Téléchargement du contenu...
[Information] Download réussi: fileName=document.pdf, contentType=application/pdf
```

---

## ⚠️ Erreurs Courantes

### Erreur: "SharePoint storage est désactivé"

**Solution:** Activer dans appsettings.json:
```json
{
  "SharePoint": {
    "UseSharePointStorage": true
  }
}
```

### Erreur: "Configuration SharePoint incomplète"

**Solution:** Vérifier SiteId et DriveId dans appsettings.json

### Erreur: "Unauthorized" (401)

**Solution:** Vérifier que:
1. Le token JWT est valide
2. Le token inclut les permissions SharePoint requises
3. L'application Azure AD a les permissions Graph API:
   - `Sites.ReadWrite.All`
   - `Files.ReadWrite.All`

### Erreur: "Fichier non trouvé" (404) au download

**Causes possibles:**
1. WebUrl incorrecte ou expirée
2. Fichier supprimé entre upload et download
3. Permissions insuffisantes sur le fichier

### Erreur: "IntegrityCheck = false"

**Causes possibles:**
1. Corruption pendant le transfert
2. Limite de taille dépassée
3. Problème de timeout

---

## 📊 Métriques de Performance

Le test cycle complet fournit des métriques utiles:

```json
{
  "uploadDurationMs": 1234,      // Temps upload
  "downloadDurationMs": 567,     // Temps download
  "totalDurationMs": 1801,       // Temps total
  "integrityCheck": true         // Vérification intégrité
}
```

**Performances attendues** (dépendent de la taille et du réseau):

| Taille Fichier | Upload (ms) | Download (ms) | Total (ms) |
|----------------|-------------|---------------|------------|
| < 1 MB | 500-1000 | 300-600 | 800-1600 |
| 1-5 MB | 1000-3000 | 600-1500 | 1600-4500 |
| 5-10 MB | 3000-6000 | 1500-3000 | 4500-9000 |
| > 10 MB | 6000+ | 3000+ | 9000+ |

---

## 🎯 Checklist de Test

### ✅ Tests Fonctionnels

- [ ] Configuration vérifiée (`/config` → `isReady: true`)
- [ ] Upload simple réussi (PDF < 1MB)
- [ ] Download réussi avec webUrl retournée
- [ ] Cycle complet réussi (`integrityCheck: true`)
- [ ] Test avec différents types de fichiers (PDF, image, Office)
- [ ] Test avec fichier volumineux (> 5MB)
- [ ] Métadonnées correctement enregistrées
- [ ] Structure de dossiers créée correctement

### ✅ Tests d'Erreurs

- [ ] Upload sans fichier → 400 Bad Request
- [ ] Download avec webUrl invalide → 404 Not Found
- [ ] Accès sans token → 401 Unauthorized
- [ ] Config incomplète → 400 Bad Request

### ✅ Tests de Performance

- [ ] Upload < 2 secondes pour fichiers < 1MB
- [ ] Download < 1 seconde pour fichiers < 1MB
- [ ] Cycle complet < 5 secondes pour fichiers < 5MB
- [ ] IntegrityCheck toujours true

---

## 🚀 Utilisation en Production

**⚠️ IMPORTANT:** Ce controller est conçu pour les **TESTS uniquement**.

**Avant production:**

1. **Désactiver ou supprimer** ce controller en production
2. Ou ajouter une restriction d'environnement:

```csharp
#if DEBUG
[ApiController]
[Route("api/[controller]")]
public class SharePointTestController : ControllerBase
{
    // ...
}
#endif
```

3. Ou restreindre l'accès aux admins uniquement:

```csharp
[Authorize(Policy = "AdminOnly")]
public class SharePointTestController : ControllerBase
{
    // ...
}
```

---

## 📚 Ressources

- **Microsoft Graph API - Files**: https://learn.microsoft.com/graph/api/resources/driveitem
- **SharePoint Sites API**: https://learn.microsoft.com/graph/api/resources/sharepoint
- **Graph Explorer**: https://developer.microsoft.com/graph/graph-explorer

---

**Créé le:** 2025-11-16
**Pour:** Tests SharePoint UploadFileAsync / DownloadByWebUrlAsync
**Controller:** SharePointTestController.cs
