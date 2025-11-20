# 📧 Email Templates - Access Request Notifications

Documentation des templates d'emails pour les notifications de demandes d'accès.

---

## 📋 Templates Disponibles

### 1. AccessRequestApproved.html
Email envoyé lorsqu'une demande d'accès est **approuvée**.

### 2. AccessRequestRejected.html
Email envoyé lorsqu'une demande d'accès est **rejetée**.

---

## ✅ Template: Access Request Approved

**Fichier:** `AccessRequestApproved.html`

**Événement:** Lorsqu'un administrateur approuve une demande d'accès

**Destinataire:** Le demandeur (utilisateur externe)

### Variables Requises

| Variable | Type | Description | Exemple |
|----------|------|-------------|---------|
| `{{recipientName}}` | string | Nom complet du destinataire | "John Doe" |
| `{{data.requestId}}` | string/Guid | Identifiant unique de la demande | "123e4567-e89b-12d3" |
| `{{data.email}}` | string | Email du demandeur | "john.doe@example.com" |
| `{{data.organization}}` | string | Organisation du demandeur | "Ministry of Finance" |
| `{{data.country}}` | string | Pays du demandeur | "Kenya" |
| `{{data.approvedDate}}` | string | Date d'approbation | "2025-11-16" |
| `{{data.approvedTime}}` | string | Heure d'approbation | "14:30 UTC" |

### Contenu

#### 🇬🇧 Section Anglaise:
- **Titre:** "Access Approved"
- **Message:** Confirmation que l'accès est approuvé
- **Détails:** Référence, email, organisation, pays, date/heure d'approbation
- **Statut:** Badge vert "✓ Approved"
- **Prochaines étapes:**
  - Accéder à la plateforme
  - Se connecter avec email
  - Compléter le profil
  - Commencer à gérer les projets
- **Bouton CTA:** "🔓 Access Portal" (lien vers login)

#### 🇫🇷 Section Française:
- **Titre:** "Accès Approuvé"
- **Message:** Confirmation que l'accès est approuvé
- **Détails:** Référence, email, organisation, pays, date/heure d'approbation
- **Statut:** Badge vert "✓ Approuvé"
- **Prochaines étapes:**
  - Accéder à la plateforme
  - Se connecter avec email
  - Compléter le profil
  - Commencer à gérer les projets
- **Bouton CTA:** "🔓 Accéder au Portail" (lien vers login)

### Design

- **Couleur principale:** Vert (#28a745, #20c997)
- **Gradient header:** Vert dégradé
- **Icône:** ✅
- **Ton:** Positif, accueillant, encourageant
- **Sections:**
  - Header avec gradient vert
  - Bloc d'informations avec fond vert clair
  - Instructions "Getting Started"
  - Bloc d'information importante (bleu)
  - Bouton vert "Access Portal"
  - Footer avec contacts

### Exemple de Code (EventHandler)

```csharp
// Dans AccessRequestApprovedEventHandler.cs
var notificationData = new
{
    requestId = accessRequest.Id.ToString(),
    email = accessRequest.Email,
    organization = accessRequest.Organization,
    country = accessRequest.Country?.NameEn ?? "N/A",
    approvedDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
    approvedTime = DateTime.UtcNow.ToString("HH:mm") + " UTC"
};

var notificationRequest = new NotificationRequest
{
    To = accessRequest.Email,
    RecipientName = $"{accessRequest.FirstName} {accessRequest.LastName}",
    EventType = NotificationEventType.AccessRequestApproved,
    Data = notificationData
};

await _notificationService.SendNotificationAsync(notificationRequest);
```

---

## ❌ Template: Access Request Rejected

**Fichier:** `AccessRequestRejected.html`

**Événement:** Lorsqu'un administrateur rejette une demande d'accès

**Destinataire:** Le demandeur (utilisateur externe)

### Variables Requises

| Variable | Type | Description | Exemple |
|----------|------|-------------|---------|
| `{{recipientName}}` | string | Nom complet du destinataire | "John Doe" |
| `{{data.requestId}}` | string/Guid | Identifiant unique de la demande | "123e4567-e89b-12d3" |
| `{{data.email}}` | string | Email du demandeur | "john.doe@example.com" |
| `{{data.organization}}` | string | Organisation du demandeur | "Ministry of Finance" |
| `{{data.country}}` | string | Pays du demandeur | "Kenya" |
| `{{data.rejectedDate}}` | string | Date de rejet | "2025-11-16" |
| `{{data.rejectedTime}}` | string | Heure de rejet | "14:30 UTC" |
| `{{data.rejectionReason}}` | string | Motif du rejet | "Incomplete documentation provided" |

### Contenu

#### 🇬🇧 Section Anglaise:
- **Titre:** "Access Request Update"
- **Message:** Information que la demande a été déclinée
- **Détails:** Référence, email, organisation, pays, date/heure de rejet
- **Statut:** Badge rouge "✗ Declined"
- **Motif du rejet:** Affiché dans un bloc jaune avec l'icône 💬
- **Prochaines étapes:**
  - Revoir le motif fourni
  - Soumettre une nouvelle demande avec plus d'informations
  - S'assurer que tous les documents sont inclus
  - Contacter le support si nécessaire
- **Bouton CTA:** "📝 Submit New Request" (lien vers formulaire)

#### 🇫🇷 Section Française:
- **Titre:** "Mise à Jour de la Demande d'Accès"
- **Message:** Information que la demande a été refusée
- **Détails:** Référence, email, organisation, pays, date/heure de rejet
- **Statut:** Badge rouge "✗ Refusé"
- **Motif du rejet:** Affiché dans un bloc jaune avec l'icône 💬
- **Prochaines étapes:**
  - Examiner le motif fourni
  - Soumettre une nouvelle demande avec plus d'informations
  - S'assurer que tous les documents sont inclus
  - Contacter le support si nécessaire
- **Bouton CTA:** "📝 Soumettre une Nouvelle Demande" (lien vers formulaire)

### Design

- **Couleur principale:** Rouge (#dc3545, #c82333)
- **Gradient header:** Rouge dégradé
- **Icône:** ❌
- **Ton:** Professionnel, empathique, constructif
- **Sections:**
  - Header avec gradient rouge
  - Bloc d'informations avec fond rouge clair
  - Bloc jaune avec motif du rejet (💬)
  - Instructions "What You Can Do Next"
  - Bloc d'information importante (bleu)
  - Bouton gris "Submit New Request"
  - Footer avec contacts

### Exemple de Code (EventHandler)

```csharp
// Dans AccessRequestRejectedEventHandler.cs
var notificationData = new
{
    requestId = accessRequest.Id.ToString(),
    email = accessRequest.Email,
    organization = accessRequest.Organization,
    country = accessRequest.Country?.NameEn ?? "N/A",
    rejectedDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
    rejectedTime = DateTime.UtcNow.ToString("HH:mm") + " UTC",
    rejectionReason = accessRequest.RejectionReason ?? "No specific reason provided"
};

var notificationRequest = new NotificationRequest
{
    To = accessRequest.Email,
    RecipientName = $"{accessRequest.FirstName} {accessRequest.LastName}",
    EventType = NotificationEventType.AccessRequestRejected,
    Data = notificationData
};

await _notificationService.SendNotificationAsync(notificationRequest);
```

---

## 🔧 Configuration Requise

### PowerAutomate Flow

Les templates utilisent le système de notification via PowerAutomate. Assurez-vous que:

1. **Flow PowerAutomate configuré** pour gérer les emails
2. **EventTypes ajoutés** dans l'enum `NotificationEventType`:

```csharp
public enum NotificationEventType
{
    // Existing...
    AccessRequestApproved,
    AccessRequestRejected
}
```

3. **Templates mappés** dans le service de notification

---

## 📊 Comparaison des Templates

| Aspect | AccessRequestApproved | AccessRequestRejected |
|--------|----------------------|----------------------|
| **Couleur** | Vert (#28a745) | Rouge (#dc3545) |
| **Ton** | Positif, accueillant | Empathique, constructif |
| **CTA** | "Access Portal" | "Submit New Request" |
| **Informations additionnelles** | Instructions de démarrage | Motif du rejet + prochaines étapes |
| **Variable spécifique** | `approvedDate/Time` | `rejectedDate/Time`, `rejectionReason` |

---

## 🎨 Éléments de Design Communs

### Structure Bilingue
- Section anglaise en premier (🇬🇧 ENGLISH)
- Séparateur visuel (• • •)
- Section française en second (🇫🇷 FRANÇAIS)

### Blocs d'Information
1. **Bloc principal:** Fond coloré (vert ou rouge) avec détails de la demande
2. **Bloc motif/info:** Fond jaune pour informations importantes
3. **Bloc aide:** Fond bleu pour informations générales

### Typography
- **Font:** 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif
- **Line height:** 1.6 pour le body, 1.8 pour les listes
- **Header:** 28px, bold
- **Sous-titres:** 18px
- **Body:** 14-16px

### Responsive
- **Max width:** 650px
- **Padding:** 20-30px
- **Borders:** 1px solid #e0e0e0
- **Border radius:** 8px pour header/footer, 4px pour blocs internes

---

## 🧪 Test des Templates

### Variables de Test - Approved

```json
{
  "recipientName": "John Doe",
  "data": {
    "requestId": "123e4567-e89b-12d3-a456-426614174000",
    "email": "john.doe@example.com",
    "organization": "Ministry of Finance",
    "country": "Kenya",
    "approvedDate": "2025-11-16",
    "approvedTime": "14:30 UTC"
  }
}
```

### Variables de Test - Rejected

```json
{
  "recipientName": "John Doe",
  "data": {
    "requestId": "123e4567-e89b-12d3-a456-426614174000",
    "email": "john.doe@example.com",
    "organization": "Ministry of Finance",
    "country": "Kenya",
    "rejectedDate": "2025-11-16",
    "rejectedTime": "14:30 UTC",
    "rejectionReason": "Incomplete documentation provided. Please include your organization registration certificate and a valid ID document."
  }
}
```

---

## 📝 Checklist d'Intégration

### Pour AccessRequestApproved:

- [ ] Template HTML créé (`AccessRequestApproved.html`)
- [ ] EventType ajouté dans enum (`AccessRequestApproved`)
- [ ] EventHandler met à jour (`AccessRequestApprovedEventHandler.cs`)
- [ ] Variables mappées correctement
- [ ] URL du portail correcte (`https://clientconnection.afdb.org/login`)
- [ ] Test avec données réelles
- [ ] Validation bilingue (EN + FR)

### Pour AccessRequestRejected:

- [ ] Template HTML créé (`AccessRequestRejected.html`)
- [ ] EventType ajouté dans enum (`AccessRequestRejected`)
- [ ] EventHandler mis à jour (`AccessRequestRejectedEventHandler.cs`)
- [ ] Variables mappées correctement (incluant `rejectionReason`)
- [ ] URL du formulaire correcte (`https://clientconnection.afdb.org/access-request`)
- [ ] Test avec données réelles
- [ ] Validation bilingue (EN + FR)
- [ ] Test avec différents motifs de rejet

---

## 🔗 Liens dans les Templates

### AccessRequestApproved:
- **Bouton CTA:** `https://clientconnection.afdb.org/login`
- **Support email:** `support@clientconnection.afdb.org`

### AccessRequestRejected:
- **Bouton CTA:** `https://clientconnection.afdb.org/access-request`
- **Support email:** `support@clientconnection.afdb.org`

**Note:** Ajustez ces URLs selon votre environnement (dev, staging, production).

---

## 💡 Best Practices

### Motifs de Rejet Clairs

Exemples de bons motifs:

✅ **Bon:**
> "Incomplete documentation provided. Please include your organization registration certificate and a valid ID document."

✅ **Bon:**
> "The email domain does not match the organization provided. Please use your official organization email address."

✅ **Bon:**
> "Unable to verify your organization. Please provide additional documentation proving your affiliation with the stated organization."

❌ **Mauvais:**
> "Invalid request"

❌ **Mauvais:**
> "Rejected"

### Ton de Communication

**Approved:**
- Enthousiaste mais professionnel
- Accueillant
- Fournir des instructions claires

**Rejected:**
- Empathique et respectueux
- Constructif (suggérer des solutions)
- Professionnel
- Ne pas être condescendant

---

## 📧 Support et Contact

Pour toute question sur ces templates:
- **Email:** support@clientconnection.afdb.org
- **Documentation:** `/EmailTemplates/ACCESS_REQUEST_EMAILS.md`

---

**Créé le:** 2025-11-16
**Templates:** AccessRequestApproved.html, AccessRequestRejected.html
**Langues:** Anglais 🇬🇧 + Français 🇫🇷
