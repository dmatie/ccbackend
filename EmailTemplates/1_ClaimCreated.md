# 📧 ClaimCreated - Templates d'Emails

Ce fichier contient les 2 templates d'emails pour l'événement **ClaimCreated**.

---

## 📋 INFORMATIONS

- **EventHandler**: `ClaimCreatedEventHandler`
- **EventType**: `ClaimCreated`
- **TemplateKey SharePoint**: `ClaimCreated`
- **Langues**: Français (FR) + English (EN)
- **Nombre d'emails envoyés**: 2
  - Email 1: À l'auteur de la réclamation (FR)
  - Email 2: Aux personnes assignées/CC (EN)

---

## 🔧 VARIABLES DISPONIBLES

Ces variables sont envoyées par le NotificationService et disponibles dans les templates:

```
{{recipientName}} - Nom complet du destinataire
{{data.claimId}} - ID de la réclamation
{{data.claimTypeFr}} - Type de réclamation en français
{{data.claimTypeEn}} - Type de réclamation en anglais
{{data.country}} - Pays
{{data.comment}} - Commentaire de l'utilisateur
{{data.authorFirstName}} - Prénom de l'auteur
{{data.authorLastName}} - Nom de l'auteur
{{data.authorEmail}} - Email de l'auteur
{{data.createdDate}} - Date (yyyy-MM-dd)
{{data.createdTime}} - Heure (HH:mm)
```

---

## 📧 EMAIL 1: AUTEUR DE LA RÉCLAMATION (FR)

### Configuration SharePoint

| Champ | Valeur |
|-------|--------|
| **TemplateKey** | `ClaimCreated` |
| **Language** | `fr` |
| **Subject** | `Confirmation de soumission - Réclamation #{{data.claimId}}` |
| **Body** | Voir le code HTML ci-dessous |

### 🎨 Design

- **Couleur principale**: Violet (#667eea → #764ba2)
- **Badge**: "En attente de traitement" (Vert)
- **Bouton**: "Voir ma réclamation"

### 📄 Corps du mail (HTML)

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; background: #f5f5f5; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }
        .content { background: #ffffff; padding: 30px; border: 1px solid #e0e0e0; border-top: none; }
        .info-box { background: #f8f9fa; padding: 20px; border-left: 4px solid #667eea; margin: 20px 0; border-radius: 4px; }
        .info-row { margin: 10px 0; }
        .label { font-weight: 600; color: #555; }
        .value { color: #333; }
        .footer { background: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #666; border-radius: 0 0 8px 8px; }
        .button { display: inline-block; padding: 12px 30px; background: #667eea; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }
        .status-badge { display: inline-block; padding: 5px 15px; background: #28a745; color: white; border-radius: 20px; font-size: 14px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin: 0; font-size: 28px;">✅ Réclamation Soumise</h1>
            <p style="margin: 10px 0 0 0; opacity: 0.9;">Votre réclamation a été enregistrée avec succès</p>
        </div>

        <div class="content">
            <p>Bonjour <strong>{{recipientName}}</strong>,</p>

            <p>Nous avons bien reçu votre réclamation. Elle est actuellement en cours d'examen par notre équipe.</p>

            <div class="info-box">
                <h3 style="margin-top: 0; color: #667eea;">📋 Détails de votre réclamation</h3>

                <div class="info-row">
                    <span class="label">Référence :</span>
                    <span class="value">#{{data.claimId}}</span>
                </div>

                <div class="info-row">
                    <span class="label">Type :</span>
                    <span class="value">{{data.claimTypeFr}}</span>
                </div>

                <div class="info-row">
                    <span class="label">Pays :</span>
                    <span class="value">{{data.country}}</span>
                </div>

                <div class="info-row">
                    <span class="label">Date de soumission :</span>
                    <span class="value">{{data.createdDate}} à {{data.createdTime}}</span>
                </div>

                <div class="info-row">
                    <span class="label">Statut :</span>
                    <span class="status-badge">En attente de traitement</span>
                </div>
            </div>

            <div style="background: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 20px 0; border-radius: 4px;">
                <p style="margin: 0;"><strong>📝 Votre commentaire :</strong></p>
                <p style="margin: 10px 0 0 0; font-style: italic;">{{data.comment}}</p>
            </div>

            <h4 style="color: #667eea; margin-top: 30px;">🔔 Prochaines étapes</h4>
            <ul style="line-height: 1.8;">
                <li>Notre équipe examinera votre réclamation dans les plus brefs délais</li>
                <li>Vous recevrez une notification dès qu'une action sera effectuée</li>
                <li>Vous pouvez suivre l'évolution de votre réclamation dans votre tableau de bord</li>
            </ul>

            <div style="text-align: center; margin-top: 30px;">
                <a href="https://clientconnection.afdb.org/claims/{{data.claimId}}" class="button">Voir ma réclamation</a>
            </div>
        </div>

        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>Cet email a été envoyé automatiquement. Merci de ne pas y répondre.</p>
            <p style="margin-top: 10px;">Pour toute question, contactez-nous à <a href="mailto:support@afdb.org">support@afdb.org</a></p>
        </div>
    </div>
</body>
</html>
```

---

## 📧 EMAIL 2: PERSONNES ASSIGNÉES/CC (EN)

### Configuration SharePoint

| Champ | Valeur |
|-------|--------|
| **TemplateKey** | `ClaimCreated` |
| **Language** | `en` |
| **Subject** | `New Claim Assigned - {{data.claimTypeEn}} - {{data.country}}` |
| **Body** | Voir le code HTML ci-dessous |

### 🎨 Design

- **Couleur principale**: Rose (#f093fb → #f5576c)
- **Badge**: "Action Required" (Rouge)
- **Bouton**: "Review Claim"

### 📄 Corps du mail (HTML)

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; background: #f5f5f5; }
        .header { background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }
        .content { background: #ffffff; padding: 30px; border: 1px solid #e0e0e0; border-top: none; }
        .info-box { background: #f8f9fa; padding: 20px; border-left: 4px solid #f5576c; margin: 20px 0; border-radius: 4px; }
        .info-row { margin: 10px 0; }
        .label { font-weight: 600; color: #555; }
        .value { color: #333; }
        .footer { background: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #666; border-radius: 0 0 8px 8px; }
        .button { display: inline-block; padding: 12px 30px; background: #f5576c; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }
        .urgent-badge { display: inline-block; padding: 5px 15px; background: #dc3545; color: white; border-radius: 20px; font-size: 14px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin: 0; font-size: 28px;">🔔 New Claim Assignment</h1>
            <p style="margin: 10px 0 0 0; opacity: 0.9;">Action Required</p>
        </div>

        <div class="content">
            <p>Hello,</p>

            <p>A new claim has been submitted and requires your attention.</p>

            <div class="info-box">
                <h3 style="margin-top: 0; color: #f5576c;">📋 Claim Details</h3>

                <div class="info-row">
                    <span class="label">Reference:</span>
                    <span class="value">#{{data.claimId}}</span>
                </div>

                <div class="info-row">
                    <span class="label">Type:</span>
                    <span class="value">{{data.claimTypeEn}}</span>
                </div>

                <div class="info-row">
                    <span class="label">Country:</span>
                    <span class="value">{{data.country}}</span>
                </div>

                <div class="info-row">
                    <span class="label">Submitted by:</span>
                    <span class="value">{{data.authorFirstName}} {{data.authorLastName}} ({{data.authorEmail}})</span>
                </div>

                <div class="info-row">
                    <span class="label">Submission date:</span>
                    <span class="value">{{data.createdDate}} at {{data.createdTime}}</span>
                </div>
            </div>

            <div style="background: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 20px 0; border-radius: 4px;">
                <p style="margin: 0;"><strong>📝 Comment:</strong></p>
                <p style="margin: 10px 0 0 0; font-style: italic;">{{data.comment}}</p>
            </div>

            <h4 style="color: #f5576c; margin-top: 30px;">⏰ Required Actions</h4>
            <ul style="line-height: 1.8;">
                <li>Review the claim details carefully</li>
                <li>Verify all supporting documents</li>
                <li>Provide your response or decision</li>
                <li>Update the claim status accordingly</li>
            </ul>

            <div style="text-align: center; margin-top: 30px;">
                <a href="https://clientconnection.afdb.org/claims/{{data.claimId}}" class="button">Review Claim</a>
            </div>
        </div>

        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>This is an automated email. Please do not reply.</p>
            <p style="margin-top: 10px;">For questions, contact <a href="mailto:support@afdb.org">support@afdb.org</a></p>
        </div>
    </div>
</body>
</html>
```

---

## ✅ CHECKLIST D'IMPLÉMENTATION

- [ ] Copier le template FR dans SharePoint avec TemplateKey="ClaimCreated" et Language="fr"
- [ ] Copier le template EN dans SharePoint avec TemplateKey="ClaimCreated" et Language="en"
- [ ] Remplacer l'URL `https://clientconnection.afdb.org` par l'URL réelle de l'application
- [ ] Remplacer `support@afdb.org` par l'email support réel
- [ ] Tester l'envoi d'email avec Power Automate
- [ ] Vérifier que toutes les variables sont correctement remplacées
- [ ] Tester l'affichage sur Outlook, Gmail et mobile

---

**Date de création**: 2025-01-06
**Auteur**: Équipe Technique AfDB
