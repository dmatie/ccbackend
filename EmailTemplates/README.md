# 📧 Templates d'Emails - Client Connection

Ce dossier contient tous les templates d'emails HTML pour les EventHandlers.

---

## 📋 LISTE DES FICHIERS

| # | Fichier | EventHandler | Langues | Status |
|---|---------|--------------|---------|--------|
| 1 | `1_ClaimCreated.md` | ClaimCreatedEventHandler | FR + EN | ✅ Créé |
| 2 | `2_ClaimResponseAdded.md` | ClaimResponseAddedEventHandler | FR + EN | ⏳ À créer |
| 3 | `3_DisbursementSubmitted.md` | DisbursementSubmittedEventHandler | FR + EN | ⏳ À créer |
| 4 | `4_DisbursementReSubmitted.md` | DisbursementReSubmittedEventHandler | FR + EN | ⏳ À créer |
| 5 | `5_DisbursementBackedToClient.md` | DisbursementBackedToClientEventHandler | FR + EN | ⏳ À créer |
| 6 | `6_DisbursementRejected.md` | DisbursementRejectedEventHandler | FR + EN | ⏳ À créer |
| 7 | `7_DisbursementApproved.md` | DisbursementApprovedEventHandler | FR + EN | ⏳ À créer |
| 8 | `8_OtpCreated.md` | CreateOtpCommandHandler | FR + EN | ⏳ À créer |

---

## 📖 FORMAT DE CHAQUE FICHIER

Chaque fichier contient:

1. **Informations générales**
   - Nom de l'EventHandler
   - TemplateKey SharePoint
   - Langues disponibles
   - Nombre d'emails envoyés

2. **Variables disponibles**
   - Liste complète des variables `{{data.xxx}}`
   - Type et description de chaque variable

3. **Template Français (FR)**
   - Configuration SharePoint (TemplateKey, Language, Subject)
   - Code HTML complet prêt à copier

4. **Template English (EN)**
   - Configuration SharePoint (TemplateKey, Language, Subject)
   - Code HTML complet prêt à copier

5. **Checklist d'implémentation**
   - Étapes pour déployer dans SharePoint
   - Points à vérifier

---

## 🚀 UTILISATION

### Étape 1: Créer la bibliothèque SharePoint

Créer une liste SharePoint "EmailTemplates" avec ces colonnes:

| Colonne | Type | Description |
|---------|------|-------------|
| TemplateKey | Texte | Identifiant unique (ex: "ClaimCreated") |
| Language | Texte | Code langue ("fr" ou "en") |
| Subject | Texte | Sujet de l'email avec variables |
| Body | Texte multiligne | Code HTML complet |

### Étape 2: Copier les templates

Pour chaque fichier:
1. Ouvrir le fichier `.md`
2. Copier la configuration SharePoint
3. Copier le code HTML
4. Créer un item dans SharePoint avec ces valeurs

### Étape 3: Configurer Power Automate

Power Automate doit:
1. Recevoir le payload du NotificationService
2. Récupérer le template depuis SharePoint selon TemplateKey + Language
3. Remplacer toutes les variables `{{xxx}}`
4. Envoyer l'email via Outlook/Office 365

---

## ⚠️ IMPORTANT

### À remplacer dans tous les templates:

- **URL de l'application**: Remplacer `https://clientconnection.afdb.org` par l'URL réelle
- **Email support**: Remplacer `support@afdb.org` par l'email support réel
- **Nom de l'organisation**: Vérifier "African Development Bank"

### Variables Power Automate:

Toutes les variables suivent ce format:
- `{{recipientName}}` - Fourni directement par NotificationService
- `{{data.xxx}}` - Dans le dictionnaire `Data` du NotificationRequest

---

## 🎨 COULEURS PAR TEMPLATE

| Template | Couleur Principale | Gradient |
|----------|-------------------|----------|
| ClaimCreated (FR) | Violet | #667eea → #764ba2 |
| ClaimCreated (EN) | Rose | #f093fb → #f5576c |
| ClaimResponseAdded | Bleu clair | #4facfe → #00f2fe |
| DisbursementSubmitted | Vert | #43e97b → #38f9d7 |
| DisbursementReSubmitted | Rose/Jaune | #fa709a → #fee140 |
| DisbursementBackedToClient | Orange | #ff9a56 → #ff6a88 |
| DisbursementRejected | Rouge | #eb3349 → #f45c43 |
| DisbursementApproved | Vert clair | #11998e → #38ef7d |
| OtpCreated | Violet | #667eea → #764ba2 |

---

## 📞 SUPPORT

Pour toute question:
- Consulter le fichier spécifique
- Vérifier les variables disponibles
- Tester dans un navigateur avant SharePoint
- Contacter l'équipe technique si besoin

---

**Dernière mise à jour**: 2025-01-06
**Version**: 1.0
