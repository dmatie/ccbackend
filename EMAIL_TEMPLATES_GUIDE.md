# 📧 GUIDE COMPLET DES TEMPLATES D'EMAILS

Ce guide contient tous les templates d'emails HTML pour les 8 EventHandlers migrés.

---

## 🎯 RÉSUMÉ RAPIDE

| # | EventHandler | Langue | Destinataire | Couleur Header |
|---|--------------|--------|--------------|----------------|
| 1 | ClaimCreated | FR | Auteur | Violet (#667eea) |
| 2 | ClaimCreated | EN | Assignés/CC | Rose (#f5576c) |
| 3 | ClaimResponseAdded | FR | Auteur | Bleu clair (#4facfe) |
| 4 | DisbursementSubmitted | FR | Créateur | Vert (#43e97b) |
| 5 | DisbursementReSubmitted | FR | Créateur | Rose/Jaune (#fa709a) |
| 6 | DisbursementBackedToClient | FR | Créateur | Orange (#ff9a56) |
| 7 | DisbursementRejected | FR | Créateur | Rouge (#eb3349) |
| 8 | DisbursementApproved | FR | Créateur | Vert clair (#11998e) |
| 9 | OtpCreated | FR | Utilisateur | Violet (#667eea) |

---

## 📋 STRUCTURE COMMUNE DES TEMPLATES

Tous les templates suivent cette structure:

```
┌─────────────────────────────────────────┐
│ HEADER (Gradient coloré + Titre)        │
├─────────────────────────────────────────┤
│ CONTENT                                 │
│  • Salutation                           │
│  • Intro                                │
│  • INFO BOX (détails principaux)        │
│  • COMMENT BOX (commentaires)           │
│  • Prochaines étapes                    │
│  • Bouton CTA                           │
├─────────────────────────────────────────┤
│ FOOTER (Infos de contact)               │
└─────────────────────────────────────────┘
```

---

## 🎨 PALETTE DE COULEURS

### Gradients utilisés:
- **Violet**: `#667eea → #764ba2` (Claims auteur, OTP)
- **Rose**: `#f093fb → #f5576c` (Claims assignés)
- **Bleu**: `#4facfe → #00f2fe` (Claims réponse)
- **Vert**: `#43e97b → #38f9d7` (Disbursement submitted)
- **Rose/Jaune**: `#fa709a → #fee140` (Disbursement resubmitted)
- **Orange**: `#ff9a56 → #ff6a88` (Disbursement backed)
- **Rouge**: `#eb3349 → #f45c43` (Disbursement rejected)
- **Vert clair**: `#11998e → #38ef7d` (Disbursement approved)

### Couleurs de badges:
- **Vert** (#28a745): Succès, Approuvé
- **Orange** (#ffc107): En attente, Warning
- **Rouge** (#dc3545): Rejeté, Urgent
- **Bleu** (#2196f3): Information

---

## 🔧 VARIABLES POWER AUTOMATE

### Variables communes (tous les templates):
```
{{recipientName}} - Nom complet du destinataire
{{data.createdDate}} - Date au format yyyy-MM-dd
{{data.createdTime}} - Heure au format HH:mm
```

### Variables Claims:
```
{{data.claimId}} - ID de la réclamation
{{data.claimTypeFr}} - Type en français
{{data.claimTypeEn}} - Type en anglais
{{data.country}} - Pays
{{data.comment}} - Commentaire initial
{{data.authorFirstName}} - Prénom auteur
{{data.authorLastName}} - Nom auteur
{{data.authorEmail}} - Email auteur
{{data.processStatus}} - Statut du process
{{data.processComment}} - Commentaire du process
{{data.processAuthorFirstName}} - Prénom responsable
{{data.processAuthorLastName}} - Nom responsable
{{data.responseDate}} - Date de réponse
{{data.responseTime}} - Heure de réponse
```

### Variables Disbursements:
```
{{data.disbursementId}} - ID du disbursement
{{data.requestNumber}} - Numéro de demande
{{data.sapCodeProject}} - Code SAP du projet
{{data.loanGrantNumber}} - Numéro Prêt/Don
{{data.disbursementTypeCode}} - Code type (A1, A2, A3, B1)
{{data.disbursementTypeName}} - Nom du type
{{data.comment}} - Commentaire
{{data.createdByFirstName}} - Prénom créateur
{{data.createdByLastName}} - Nom créateur
{{data.createdByEmail}} - Email créateur
{{data.submittedDate}} - Date soumission
{{data.submittedTime}} - Heure soumission
{{data.resubmittedDate}} - Date resoumission
{{data.resubmittedTime}} - Heure resoumission
{{data.processedByFirstName}} - Prénom responsable
{{data.processedByLastName}} - Nom responsable
{{data.processedByEmail}} - Email responsable
{{data.approvedByFirstName}} - Prénom approbateur
{{data.approvedByLastName}} - Nom approbateur
{{data.approvedByEmail}} - Email approbateur
{{data.rejectedByFirstName}} - Prénom rejeteur
{{data.rejectedByLastName}} - Nom rejeteur
{{data.rejectedByEmail}} - Email rejeteur
{{data.rejectionComment}} - Commentaire de rejet
{{data.backedDate}} - Date renvoi
{{data.backedTime}} - Heure renvoi
{{data.approvedDate}} - Date approbation
{{data.approvedTime}} - Heure approbation
{{data.rejectedDate}} - Date rejet
{{data.rejectedTime}} - Heure rejet
```

### Variables OTP:
```
{{data.email}} - Email de l'utilisateur
{{data.otpCode}} - Code à 6 chiffres
{{data.expiresInMinutes}} - Durée de validité (10)
```

---

## 📝 SUJETS DES EMAILS

| Template | Sujet |
|----------|-------|
| ClaimCreated (FR) | `Confirmation de soumission - Réclamation #{data.claimId}` |
| ClaimCreated (EN) | `New Claim Assigned - {{data.claimTypeEn}} - {{data.country}}` |
| ClaimResponseAdded | `Réponse ajoutée à votre réclamation - {{data.claimTypeFr}}` |
| DisbursementSubmitted | `Demande de décaissement soumise - {{data.requestNumber}}` |
| DisbursementReSubmitted | `Demande de décaissement resoumise - {{data.requestNumber}}` |
| DisbursementBackedToClient | `Action requise - Modifications demandées sur {{data.requestNumber}}` |
| DisbursementRejected | `Demande de décaissement rejetée - {{data.requestNumber}}` |
| DisbursementApproved | `✅ Demande de décaissement approuvée - {{data.requestNumber}}` |
| OtpCreated | `Code de vérification - Client Connection` |

---

## 🔗 BOUTONS CTA (Call-to-Action)

Chaque template contient un bouton avec un lien vers l'application.

**Format du lien**: `[URL_APP]/section/{{data.id}}`

Exemples:
- Claims: `[URL_APP]/claims/{{data.claimId}}`
- Disbursements: `[URL_APP]/disbursements/{{data.disbursementId}}`
- Disbursement Edit: `[URL_APP]/disbursements/{{data.disbursementId}}/edit`

**⚠️ Important**: Remplacer `[URL_APP]` par l'URL réelle de votre application.

---

## 📦 COMMENT UTILISER DANS SHAREPOINT

### 1. Créer la bibliothèque SharePoint

Créer une liste SharePoint avec ces colonnes:

| Colonne | Type | Description |
|---------|------|-------------|
| TemplateKey | Texte | Clé unique (ex: "ClaimCreated") |
| Language | Texte | Code langue ("fr" ou "en") |
| Subject | Texte | Sujet de l'email |
| Body | Texte multiligne | Corps HTML de l'email |

### 2. Ajouter les templates

Pour chaque template, créer un item avec:
- **TemplateKey**: Le nom de l'événement (ex: `ClaimCreated`, `DisbursementApproved`)
- **Language**: `fr` ou `en`
- **Subject**: Le sujet de l'email (avec variables {{data.xxx}})
- **Body**: Le code HTML complet

### 3. Configuration Power Automate

```javascript
// 1. Récupérer le template depuis SharePoint
GET https://[SITE]/lists/EmailTemplates/items?
  $filter=TemplateKey eq 'ClaimCreated' and Language eq 'fr'

// 2. Remplacer les variables
let subject = template.Subject;
let body = template.Body;

// Remplacer {{recipientName}}
subject = subject.replace('{{recipientName}}', userData.name);
body = body.replace('{{recipientName}}', userData.name);

// Remplacer toutes les {{data.xxx}}
for (let key in notificationData) {
    subject = subject.replace(`{{data.${key}}}`, notificationData[key]);
    body = body.replace(new RegExp(`{{data.${key}}}`, 'g'), notificationData[key]);
}

// 3. Envoyer l'email
Send Email (V2)
  To: recipient
  Subject: subject
  Body: body
  IsHtml: Yes
```

---

## ✅ CHECKLIST AVANT DÉPLOIEMENT

- [ ] Remplacer `[URL_APP]` par l'URL réelle
- [ ] Remplacer `support@afdb.org` par l'email support réel
- [ ] Tester l'affichage sur Outlook Desktop
- [ ] Tester l'affichage sur Outlook Web
- [ ] Tester l'affichage sur Gmail
- [ ] Tester l'affichage sur mobile
- [ ] Vérifier les contrastes de couleurs (accessibilité)
- [ ] Tester toutes les variables {{data.xxx}}
- [ ] Vérifier les liens des boutons
- [ ] Valider les sujets des emails

---

## 🎨 PERSONNALISATION

### Modifier les couleurs:

Dans chaque template, chercher la section `<style>` et modifier:
- `.header { background: linear-gradient(...)` - Couleur de l'en-tête
- `.button { background: #xxx }` - Couleur du bouton
- `.info-box { border-left: 4px solid #xxx }` - Bordure des encadrés

### Modifier le logo:

Ajouter un logo dans le header:
```html
<div class="header">
    <img src="[LOGO_URL]" alt="AfDB Logo" style="max-width: 150px; margin-bottom: 20px;">
    <h1>Titre...</h1>
</div>
```

### Modifier le footer:

Personnaliser les informations de contact:
```html
<div class="footer">
    <p><strong>Votre Application</strong> | Votre Organisation</p>
    <p>Email: <a href="mailto:contact@example.com">contact@example.com</a></p>
    <p>Téléphone: +xxx xxx xxx xxx</p>
</div>
```

---

## 📞 SUPPORT

Pour toute question sur les templates:
1. Consulter ce guide
2. Vérifier la structure HTML
3. Tester les variables Power Automate
4. Contacter l'équipe technique

---

**Date de création**: 2025-01-06  
**Version**: 1.0  
**Auteur**: Équipe Technique AfDB

