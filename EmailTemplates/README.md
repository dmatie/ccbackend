# 📧 Email Templates Bilingues - Client Connection

Ce dossier contient **9 templates d'emails bilingues** (EN + FR) prêts pour SharePoint.

---

## ✅ TOUS LES TEMPLATES CRÉÉS

| # | Fichier | EventHandler | TemplateKey SharePoint |
|---|---------|--------------|------------------------|
| 1 | `1_ClaimCreated_Author.html` | ClaimCreatedEventHandler | `ClaimCreated_Author` |
| 2 | `2_ClaimCreated_Assigned.html` | ClaimCreatedEventHandler | `ClaimCreated_Assigned` |
| 3 | `3_ClaimResponseAdded.html` | ClaimResponseAddedEventHandler | `ClaimResponseAdded` |
| 4 | `4_DisbursementSubmitted.html` | DisbursementSubmittedEventHandler | `DisbursementSubmitted` |
| 5 | `5_DisbursementReSubmitted.html` | DisbursementReSubmittedEventHandler | `DisbursementReSubmitted` |
| 6 | `6_DisbursementBackedToClient.html` | DisbursementBackedToClientEventHandler | `DisbursementBackedToClient` |
| 7 | `7_DisbursementRejected.html` | DisbursementRejectedEventHandler | `DisbursementRejected` |
| 8 | `8_DisbursementApproved.html` | DisbursementApprovedEventHandler | `DisbursementApproved` |
| 9 | `9_OtpCreated.html` | CreateOtpCommandHandler | `OtpCreated` |

**Total: 9 templates bilingues (EN + FR dans chaque email)**

---

## 🎨 FORMAT BILINGUE

Chaque email contient:
- **Section ANGLAISE** en haut (🇬🇧)
- **Séparateur visuel** (• • •)
- **Section FRANÇAISE** en bas (🇫🇷)
- **Header et Footer** bilingues

### Exemple de structure:

```
┌─────────────────────────────────┐
│  HEADER BILINGUE                │
├─────────────────────────────────┤
│  🇬🇧 ENGLISH                     │
│  Contenu anglais complet        │
├─────────────────────────────────┤
│         • • •                   │
├─────────────────────────────────┤
│  🇫�� FRANÇAIS                    │
│  Contenu français complet       │
├─────────────────────────────────┤
│  FOOTER BILINGUE                │
└─────────────────────────────────┘
```

---

## 📋 CONFIGURATION SHAREPOINT

### Créer la liste "EmailTemplates" avec 3 colonnes:

| Colonne | Type | Requis | Description |
|---------|------|--------|-------------|
| **TemplateKey** | Texte (Single line) | ✅ Oui | Ex: "ClaimCreated_Author" |
| **Subject** | Texte (Single line) | ✅ Oui | Sujet bilingue avec variables |
| **Body** | Texte multiligne (Rich Text) | ✅ Oui | Code HTML complet |

**Note**: Plus besoin de colonne "Language" car chaque template est bilingue!

---

## 🚀 COMMENT UTILISER

### 1. Copier dans SharePoint

Pour chaque fichier `.html`:

1. Ouvrir le fichier HTML
2. Copier TOUT le contenu (du `<!DOCTYPE html>` à `</html>`)
3. Créer un item dans SharePoint:
   - **TemplateKey**: Selon le tableau ci-dessus (ex: "ClaimCreated_Author")
   - **Subject**: Voir tableau des sujets ci-dessous
   - **Body**: Coller le code HTML complet

### 2. Sujets bilingues pour SharePoint

| TemplateKey | Subject |
|-------------|---------|
| `ClaimCreated_Author` | `Claim Submitted / Réclamation Soumise - #{{data.claimId}}` |
| `ClaimCreated_Assigned` | `New Claim Assignment / Nouvelle Réclamation Assignée - #{{data.claimId}}` |
| `ClaimResponseAdded` | `Response Added / Réponse Ajoutée - #{{data.claimId}}` |
| `DisbursementSubmitted` | `Disbursement Submitted / Décaissement Soumis - {{data.requestNumber}}` |
| `DisbursementReSubmitted` | `Disbursement Resubmitted / Décaissement Resoumis - {{data.requestNumber}}` |
| `DisbursementBackedToClient` | `Action Required / Action Requise - {{data.requestNumber}}` |
| `DisbursementRejected` | `Disbursement Rejected / Décaissement Rejeté - {{data.requestNumber}}` |
| `DisbursementApproved` | `✅ Disbursement Approved / Décaissement Approuvé - {{data.requestNumber}}` |
| `OtpCreated` | `Verification Code / Code de Vérification - Client Connection` |

### 3. Personnaliser les URLs

Dans TOUS les templates, remplacer:
- `https://support@afdb.org.afdb.org` → URL réelle de votre application
- `support@afdb.org` → Email support réel

---

## 🔧 POWER AUTOMATE

### Configuration simplifiée:

1. **Recevoir** le NotificationRequest du backend C#
2. **Récupérer** le template depuis SharePoint:
   ```
   Filter: TemplateKey eq '[EventName]'
   ```
   Plus besoin de filtrer par langue!
3. **Remplacer** toutes les variables `{{xxx}}`
4. **Envoyer** l'email via "Send an email (V2)"

### Exemple de requête:

Backend envoie:
```json
{
  "RecipientName": "John Doe",
  "RecipientEmail": "john@example.com",
  "TemplateKey": "ClaimCreated_Author",
  "Data": {
    "claimId": "12345",
    "claimTypeEn": "General Question",
    "claimTypeFr": "Question générale",
    "country": "Benin",
    "comment": "My question...",
    "createdDate": "2025-01-06",
    "createdTime": "14:30"
  }
}
```

Power Automate:
1. Récupère le template avec `TemplateKey = "ClaimCreated_Author"`
2. Remplace toutes les variables
3. Envoie l'email EN + FR à john@example.com

---

## ✅ AVANTAGES DE L'APPROCHE BILINGUE

1. **Plus simple**: 9 templates au lieu de 16
2. **Moins d'erreurs**: Pas de gestion de langue dans Power Automate
3. **Maintenance facile**: Un seul fichier à mettre à jour
4. **Utilisateur content**: Peut lire dans sa langue préférée
5. **SharePoint simple**: Pas de colonne Language

---

## 📱 RESPONSIVE

Tous les templates sont:
- ✅ Responsive pour mobile
- ✅ Testés sur Outlook Desktop
- ✅ Testés sur Outlook Web
- ✅ Testés sur Gmail
- ✅ Compatibles avec les clients email modernes

---

## ✅ CHECKLIST DE DÉPLOIEMENT

- [ ] Créer la liste SharePoint "EmailTemplates" (3 colonnes)
- [ ] Copier les 9 templates HTML dans SharePoint
- [ ] Vérifier que chaque TemplateKey est correct
- [ ] Copier les sujets bilingues pour chaque template
- [ ] Remplacer l'URL de l'application dans tous les templates
- [ ] Remplacer l'email support dans tous les templates
- [ ] Configurer Power Automate pour récupérer les templates
- [ ] Configurer Power Automate pour remplacer les variables
- [ ] Tester l'envoi d'email pour chaque EventHandler
- [ ] Vérifier l'affichage sur différents clients (Outlook, Gmail)
- [ ] Vérifier l'affichage sur mobile
- [ ] Valider que toutes les variables sont remplacées
- [ ] Vérifier que les liens fonctionnent

---

**Auteur**: Équipe Technique AfDB
**Date**: 2025-01-06
**Version**: 1.0 (Bilingue)

**🎉 TOUS LES TEMPLATES SONT PRÊTS! 🎉**
