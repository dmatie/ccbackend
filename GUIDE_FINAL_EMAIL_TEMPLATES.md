# 📧 GUIDE FINAL - Templates d'Emails Client Connection

## ✅ TOUT EST PRÊT!

Vous disposez maintenant de **TOUS les templates d'emails** pour les 8 EventHandlers en versions française et anglaise.

**Total: 16 templates complets** prêts à être copiés dans SharePoint!

---

## 📁 FICHIERS DISPONIBLES

### Fichier principal (RECOMMANDÉ)
- **`ALL_EMAIL_TEMPLATES.md`** - Contient les templates 1, 2 et 3 COMPLETS

### Fichiers complémentaires
- **`EMAIL_TEMPLATES_4_to_8.md`** - Contient les templates 4 et 5 (DisbursementReSubmitted, DisbursementBackedToClient)
- **`EMAIL_TEMPLATES_6_7_8.md`** - Contient les templates 6, 7 et 8 (DisbursementRejected, DisbursementApproved, OtpCreated)

---

## 📊 LISTE COMPLÈTE DES TEMPLATES

| # | EventHandler | TemplateKey | FR | EN |
|---|--------------|-------------|----|----|
| 1 | ClaimCreated (Auteur) | ClaimCreated | ✅ | ✅ |
| 1bis | ClaimCreated (Assignés) | ClaimCreated | ✅ | ✅ |
| 2 | ClaimResponseAdded | ClaimResponseAdded | ✅ | ✅ |
| 3 | DisbursementSubmitted | DisbursementSubmitted | ✅ | ✅ |
| 4 | DisbursementReSubmitted | DisbursementReSubmitted | ✅ | ✅ |
| 5 | DisbursementBackedToClient | DisbursementBackedToClient | ✅ | ✅ |
| 6 | DisbursementRejected | DisbursementRejected | ✅ | ✅ |
| 7 | DisbursementApproved | DisbursementApproved | ✅ | ✅ |
| 8 | OtpCreated | OtpCreated | ✅ | ✅ |

**Total: 16 templates (8 EventHandlers × 2 langues)**

---

## 🚀 ÉTAPES D'IMPLÉMENTATION

### Étape 1: Créer la liste SharePoint

Créez une liste SharePoint nommée **"EmailTemplates"** avec ces 4 colonnes:

| Colonne | Type | Requis | Description |
|---------|------|--------|-------------|
| **TemplateKey** | Texte (Single line) | ✅ Oui | Ex: "ClaimCreated" |
| **Language** | Texte (Single line) | ✅ Oui | "fr" ou "en" |
| **Subject** | Texte (Single line) | ✅ Oui | Sujet avec variables {{xxx}} |
| **Body** | Texte multiligne (Rich Text) | ✅ Oui | Code HTML complet |

### Étape 2: Copier les templates dans SharePoint

Pour chaque template:

1. **Ouvrir le fichier markdown** correspondant
2. **Trouver la section "Config SharePoint"**
3. **Copier les valeurs** :
   - TemplateKey (ex: "ClaimCreated")
   - Language ("fr" ou "en")
   - Subject (avec les variables)
   - Body (tout le code HTML)
4. **Créer un item** dans SharePoint avec ces valeurs

### Étape 3: Personnaliser les templates

Dans TOUS les templates, remplacer:

- `https://clientconnection.afdb.org` → **URL réelle de votre application**
- `support@afdb.org` → **Email support réel**

### Étape 4: Configurer Power Automate

Power Automate doit:

1. **Recevoir** le NotificationRequest du backend C#
2. **Récupérer** le template depuis SharePoint:
   ```
   Filter: TemplateKey eq '[EventName]' and Language eq '[fr/en]'
   ```
3. **Remplacer** toutes les variables `{{xxx}}` par les vraies valeurs
4. **Envoyer** l'email via "Send an email (V2)"

---

## 🎯 EXEMPLE D'UTILISATION

### Pour ClaimCreated (FR):

**Dans SharePoint:**
```
TemplateKey: ClaimCreated
Language: fr
Subject: Confirmation de soumission - Réclamation #{{data.claimId}}
Body: [Code HTML du template]
```

**Power Automate reçoit:**
```json
{
  "RecipientName": "John Doe",
  "RecipientEmail": "john@example.com",
  "TemplateKey": "ClaimCreated",
  "Language": "fr",
  "Data": {
    "claimId": "12345",
    "claimTypeFr": "Question générale",
    "country": "Bénin",
    "comment": "Ma question...",
    "createdDate": "2025-01-06",
    "createdTime": "14:30"
  }
}
```

**Power Automate:**
1. Récupère le template SharePoint (TemplateKey="ClaimCreated", Language="fr")
2. Remplace:
   - `{{recipientName}}` → "John Doe"
   - `{{data.claimId}}` → "12345"
   - `{{data.claimTypeFr}}` → "Question générale"
   - etc.
3. Envoie l'email à john@example.com

---

## 🎨 COULEURS PAR TEMPLATE

| Template | Gradient | Badge |
|----------|----------|-------|
| ClaimCreated FR | Violet (#667eea → #764ba2) | Vert |
| ClaimCreated EN | Rose (#f093fb → #f5576c) | Rouge |
| ClaimResponseAdded | Bleu (#4facfe → #00f2fe) | Vert |
| DisbursementSubmitted | Vert (#43e97b → #38f9d7) | Orange |
| DisbursementReSubmitted | Rose/Jaune (#fa709a → #fee140) | Orange |
| DisbursementBackedToClient | Orange (#ff9a56 → #ff6a88) | Orange |
| DisbursementRejected | Rouge (#eb3349 → #f45c43) | Rouge |
| DisbursementApproved | Vert clair (#11998e → #38ef7d) | Vert |
| OtpCreated | Violet (#667eea → #764ba2) | - |

---

## 📝 VARIABLES PAR EVENTHANDLER

### Claims (ClaimCreated, ClaimResponseAdded)
```
{{recipientName}}, {{data.claimId}}, {{data.claimTypeFr}}, {{data.claimTypeEn}},
{{data.country}}, {{data.comment}}, {{data.authorFirstName}}, {{data.authorLastName}},
{{data.authorEmail}}, {{data.createdDate}}, {{data.createdTime}}, {{data.processStatus}},
{{data.processComment}}, {{data.processAuthorFirstName}}, {{data.processAuthorLastName}},
{{data.responseDate}}, {{data.responseTime}}
```

### Disbursements (Tous)
```
{{recipientName}}, {{data.disbursementId}}, {{data.requestNumber}}, {{data.sapCodeProject}},
{{data.loanGrantNumber}}, {{data.disbursementTypeCode}}, {{data.disbursementTypeName}},
{{data.comment}}, {{data.submittedDate}}, {{data.submittedTime}}, {{data.resubmittedDate}},
{{data.resubmittedTime}}, {{data.processedByFirstName}}, {{data.processedByLastName}},
{{data.processedByEmail}}, {{data.approvedByFirstName}}, {{data.approvedByLastName}},
{{data.rejectedByFirstName}}, {{data.rejectedByLastName}}, {{data.rejectedByEmail}},
{{data.rejectionComment}}, {{data.backedDate}}, {{data.backedTime}}, {{data.approvedDate}},
{{data.approvedTime}}, {{data.rejectedDate}}, {{data.rejectedTime}}
```

### OTP
```
{{recipientName}}, {{data.email}}, {{data.otpCode}}, {{data.expiresInMinutes}},
{{data.createdDate}}, {{data.createdTime}}
```

---

## ✅ CHECKLIST DE DÉPLOIEMENT

- [ ] Créer la liste SharePoint "EmailTemplates"
- [ ] Copier les 16 templates dans SharePoint
- [ ] Remplacer l'URL de l'application dans tous les templates
- [ ] Remplacer l'email support dans tous les templates
- [ ] Configurer Power Automate pour récupérer les templates
- [ ] Configurer Power Automate pour remplacer les variables
- [ ] Tester l'envoi d'email pour chaque EventHandler
- [ ] Vérifier l'affichage sur Outlook Desktop
- [ ] Vérifier l'affichage sur Outlook Web
- [ ] Vérifier l'affichage sur Gmail
- [ ] Vérifier l'affichage sur mobile (iOS/Android)
- [ ] Valider que toutes les variables sont remplacées
- [ ] Vérifier que les liens fonctionnent
- [ ] Tester avec des caractères spéciaux dans les commentaires

---

## 🆘 RÉSOLUTION DE PROBLÈMES

### Les variables ne sont pas remplacées
- Vérifier que Power Automate reçoit bien les données
- Vérifier l'orthographe exacte des variables (sensible à la casse)
- Vérifier que les `{{}}` sont présents

### L'email n'est pas bien formaté
- Vérifier que le champ Body dans SharePoint est en "Rich Text"
- Vérifier qu'il n'y a pas de caractères spéciaux cassés dans le HTML

### Le template n'est pas trouvé
- Vérifier le TemplateKey exact (sensible à la casse)
- Vérifier le Language ("fr" ou "en")
- Vérifier le filtre dans Power Automate

---

## 📞 SUPPORT

Pour toute question:
1. Consulter ce guide
2. Vérifier les fichiers markdown des templates
3. Tester les variables dans Power Automate
4. Contacter l'équipe technique

---

**Auteur**: Équipe Technique AfDB
**Date**: 2025-01-06
**Version**: 1.0

**🎉 TOUS LES TEMPLATES SONT PRÊTS À L'EMPLOI! 🎉**
