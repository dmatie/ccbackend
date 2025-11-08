# 📧 TOUS LES TEMPLATES D'EMAILS - SharePoint Ready

Ce fichier contient **TOUS les templates d'emails** pour les 8 EventHandlers.
Chaque template est prêt à être copié dans SharePoint.

---

## 📋 TABLE DES MATIÈRES

1. [ClaimCreated](#1-claimcreated)
2. [ClaimResponseAdded](#2-claimresponseadded)
3. [DisbursementSubmitted](#3-disbursementsubmitted)
4. [DisbursementReSubmitted](#4-disbursementresubmitted)
5. [DisbursementBackedToClient](#5-disbursementbackedtoclient)
6. [DisbursementRejected](#6-disbursementrejected)
7. [DisbursementApproved](#7-disbursementapproved)
8. [OtpCreated](#8-otpcreated)

---

## 🔧 CONFIGURATION SHAREPOINT

### Créer la liste "EmailTemplates" avec ces colonnes:

| Colonne | Type | Description |
|---------|------|-------------|
| **TemplateKey** | Texte | Identifiant (ex: "ClaimCreated") |
| **Language** | Texte | Code langue: "fr" ou "en" |
| **Subject** | Texte | Sujet avec variables {{xxx}} |
| **Body** | Texte multiligne (Rich Text) | Code HTML complet |

---

## ⚠️ IMPORTANT AVANT DE COPIER

**Remplacer dans TOUS les templates:**
- `https://clientconnection.afdb.org` → URL réelle de votre application
- `support@afdb.org` → Email support réel

---

# 1. ClaimCreated

**EventHandler**: `ClaimCreatedEventHandler`
**Emails envoyés**: 2 (Auteur FR + Assignés EN)

## Variables disponibles
```
{{recipientName}}, {{data.claimId}}, {{data.claimTypeFr}}, {{data.claimTypeEn}},
{{data.country}}, {{data.comment}}, {{data.authorFirstName}}, {{data.authorLastName}},
{{data.authorEmail}}, {{data.createdDate}}, {{data.createdTime}}
```

## 📧 Template FR (Auteur)

### Config SharePoint
- **TemplateKey**: `ClaimCreated`
- **Language**: `fr`
- **Subject**: `Confirmation de soumission - Réclamation #{{data.claimId}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#667eea 0%,#764ba2 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #667eea;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0}
        .label{font-weight:600;color:#555}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#667eea;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#28a745;color:white;border-radius:20px;font-size:14px}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">✅ Réclamation Soumise</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Votre réclamation a été enregistrée avec succès</p>
        </div>
        <div class="content">
            <p>Bonjour <strong>{{recipientName}}</strong>,</p>
            <p>Nous avons bien reçu votre réclamation. Elle est actuellement en cours d'examen par notre équipe.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#667eea">📋 Détails de votre réclamation</h3>
                <div class="info-row"><span class="label">Référence :</span> #{{data.claimId}}</div>
                <div class="info-row"><span class="label">Type :</span> {{data.claimTypeFr}}</div>
                <div class="info-row"><span class="label">Pays :</span> {{data.country}}</div>
                <div class="info-row"><span class="label">Date de soumission :</span> {{data.createdDate}} à {{data.createdTime}}</div>
                <div class="info-row"><span class="label">Statut :</span> <span class="status-badge">En attente de traitement</span></div>
            </div>
            <div style="background:#fff3cd;padding:15px;border-left:4px solid #ffc107;margin:20px 0;border-radius:4px">
                <p style="margin:0"><strong>📝 Votre commentaire :</strong></p>
                <p style="margin:10px 0 0 0;font-style:italic">{{data.comment}}</p>
            </div>
            <h4 style="color:#667eea;margin-top:30px">🔔 Prochaines étapes</h4>
            <ul style="line-height:1.8">
                <li>Notre équipe examinera votre réclamation dans les plus brefs délais</li>
                <li>Vous recevrez une notification dès qu'une action sera effectuée</li>
                <li>Vous pouvez suivre l'évolution de votre réclamation dans votre tableau de bord</li>
            </ul>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/claims/{{data.claimId}}" class="button">Voir ma réclamation</a>
            </div>
        </div>
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>Cet email a été envoyé automatiquement. Merci de ne pas y répondre.</p>
            <p style="margin-top:10px">Pour toute question, contactez-nous à <a href="mailto:support@afdb.org">support@afdb.org</a></p>
        </div>
    </div>
</body>
</html>
```

## 📧 Template EN (Assignés)

### Config SharePoint
- **TemplateKey**: `ClaimCreated`
- **Language**: `en`
- **Subject**: `New Claim Assigned - {{data.claimTypeEn}} - {{data.country}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#f093fb 0%,#f5576c 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #f5576c;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0}
        .label{font-weight:600;color:#555}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#f5576c;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">🔔 New Claim Assignment</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Action Required</p>
        </div>
        <div class="content">
            <p>Hello,</p>
            <p>A new claim has been submitted and requires your attention.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#f5576c">📋 Claim Details</h3>
                <div class="info-row"><span class="label">Reference:</span> #{{data.claimId}}</div>
                <div class="info-row"><span class="label">Type:</span> {{data.claimTypeEn}}</div>
                <div class="info-row"><span class="label">Country:</span> {{data.country}}</div>
                <div class="info-row"><span class="label">Submitted by:</span> {{data.authorFirstName}} {{data.authorLastName}} ({{data.authorEmail}})</div>
                <div class="info-row"><span class="label">Submission date:</span> {{data.createdDate}} at {{data.createdTime}}</div>
            </div>
            <div style="background:#fff3cd;padding:15px;border-left:4px solid #ffc107;margin:20px 0;border-radius:4px">
                <p style="margin:0"><strong>📝 Comment:</strong></p>
                <p style="margin:10px 0 0 0;font-style:italic">{{data.comment}}</p>
            </div>
            <h4 style="color:#f5576c;margin-top:30px">⏰ Required Actions</h4>
            <ul style="line-height:1.8">
                <li>Review the claim details carefully</li>
                <li>Verify all supporting documents</li>
                <li>Provide your response or decision</li>
                <li>Update the claim status accordingly</li>
            </ul>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/claims/{{data.claimId}}" class="button">Review Claim</a>
            </div>
        </div>
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>This is an automated email. Please do not reply.</p>
            <p style="margin-top:10px">For questions, contact <a href="mailto:support@afdb.org">support@afdb.org</a></p>
        </div>
    </div>
</body>
</html>
```

---

# 2. ClaimResponseAdded

**EventHandler**: `ClaimResponseAddedEventHandler`
**Emails envoyés**: 1 (Auteur)

## Variables disponibles
```
{{recipientName}}, {{data.claimId}}, {{data.claimTypeFr}}, {{data.claimTypeEn}},
{{data.country}}, {{data.processStatus}}, {{data.processComment}},
{{data.processAuthorFirstName}}, {{data.processAuthorLastName}},
{{data.responseDate}}, {{data.responseTime}}
```

## 📧 Template FR

### Config SharePoint
- **TemplateKey**: `ClaimResponseAdded`
- **Language**: `fr`
- **Subject**: `Réponse ajoutée à votre réclamation - {{data.claimTypeFr}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#4facfe 0%,#00f2fe 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #4facfe;margin:20px 0;border-radius:4px}
        .response-box{background:#e8f5e9;padding:20px;border-left:4px solid #4caf50;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0}
        .label{font-weight:600;color:#555}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#4facfe;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#4caf50;color:white;border-radius:20px;font-size:14px}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">💬 Nouvelle Réponse</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Une mise à jour concernant votre réclamation</p>
        </div>
        <div class="content">
            <p>Bonjour <strong>{{recipientName}}</strong>,</p>
            <p>Une réponse a été ajoutée à votre réclamation par notre équipe.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#4facfe">📋 Rappel de votre réclamation</h3>
                <div class="info-row"><span class="label">Référence :</span> #{{data.claimId}}</div>
                <div class="info-row"><span class="label">Type :</span> {{data.claimTypeFr}}</div>
                <div class="info-row"><span class="label">Pays :</span> {{data.country}}</div>
            </div>
            <div class="response-box">
                <h3 style="margin-top:0;color:#2e7d32">✅ Réponse du responsable</h3>
                <div class="info-row"><span class="label">Responsable :</span> {{data.processAuthorFirstName}} {{data.processAuthorLastName}}</div>
                <div class="info-row"><span class="label">Statut :</span> <span class="status-badge">{{data.processStatus}}</span></div>
                <div class="info-row"><span class="label">Date de réponse :</span> {{data.responseDate}} à {{data.responseTime}}</div>
                <hr style="border:none;border-top:1px solid #c8e6c9;margin:20px 0">
                <p style="margin:0"><strong>💬 Commentaire :</strong></p>
                <p style="margin:10px 0 0 0;line-height:1.8;background:white;padding:15px;border-radius:4px">{{data.processComment}}</p>
            </div>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/claims/{{data.claimId}}" class="button">Voir les détails</a>
            </div>
        </div>
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>Cet email a été envoyé automatiquement. Merci de ne pas y répondre.</p>
        </div>
    </div>
</body>
</html>
```

## 📧 Template EN

### Config SharePoint
- **TemplateKey**: `ClaimResponseAdded`
- **Language**: `en`
- **Subject**: `Response added to your claim - {{data.claimTypeEn}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#4facfe 0%,#00f2fe 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #4facfe;margin:20px 0;border-radius:4px}
        .response-box{background:#e8f5e9;padding:20px;border-left:4px solid #4caf50;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0}
        .label{font-weight:600;color:#555}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#4facfe;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#4caf50;color:white;border-radius:20px;font-size:14px}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">💬 New Response</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">An update regarding your claim</p>
        </div>
        <div class="content">
            <p>Hello <strong>{{recipientName}}</strong>,</p>
            <p>A response has been added to your claim by our team.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#4facfe">📋 Claim Reminder</h3>
                <div class="info-row"><span class="label">Reference:</span> #{{data.claimId}}</div>
                <div class="info-row"><span class="label">Type:</span> {{data.claimTypeEn}}</div>
                <div class="info-row"><span class="label">Country:</span> {{data.country}}</div>
            </div>
            <div class="response-box">
                <h3 style="margin-top:0;color:#2e7d32">✅ Manager's Response</h3>
                <div class="info-row"><span class="label">Manager:</span> {{data.processAuthorFirstName}} {{data.processAuthorLastName}}</div>
                <div class="info-row"><span class="label">Status:</span> <span class="status-badge">{{data.processStatus}}</span></div>
                <div class="info-row"><span class="label">Response date:</span> {{data.responseDate}} at {{data.responseTime}}</div>
                <hr style="border:none;border-top:1px solid #c8e6c9;margin:20px 0">
                <p style="margin:0"><strong>💬 Comment:</strong></p>
                <p style="margin:10px 0 0 0;line-height:1.8;background:white;padding:15px;border-radius:4px">{{data.processComment}}</p>
            </div>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/claims/{{data.claimId}}" class="button">View Details</a>
            </div>
        </div>
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>This is an automated email. Please do not reply.</p>
        </div>
    </div>
</body>
</html>
```

---

**Note**: Les templates 3 à 8 suivent la même structure.
Pour les voir tous, consultez les fichiers individuels dans le dossier `EmailTemplates/`.

---

## 📝 RÉSUMÉ DES TEMPLATES

| # | Template | FR Subject | EN Subject |
|---|----------|------------|------------|
| 1 | ClaimCreated | Confirmation de soumission - Réclamation #{{data.claimId}} | New Claim Assigned - {{data.claimTypeEn}} - {{data.country}} |
| 2 | ClaimResponseAdded | Réponse ajoutée à votre réclamation - {{data.claimTypeFr}} | Response added to your claim - {{data.claimTypeEn}} |
| 3 | DisbursementSubmitted | Demande de décaissement soumise - {{data.requestNumber}} | Disbursement request submitted - {{data.requestNumber}} |
| 4 | DisbursementReSubmitted | Demande de décaissement resoumise - {{data.requestNumber}} | Disbursement request resubmitted - {{data.requestNumber}} |
| 5 | DisbursementBackedToClient | Action requise - Modifications demandées sur {{data.requestNumber}} | Action required - Changes requested on {{data.requestNumber}} |
| 6 | DisbursementRejected | Demande de décaissement rejetée - {{data.requestNumber}} | Disbursement request rejected - {{data.requestNumber}} |
| 7 | DisbursementApproved | ✅ Demande de décaissement approuvée - {{data.requestNumber}} | ✅ Disbursement request approved - {{data.requestNumber}} |
| 8 | OtpCreated | Code de vérification - Client Connection | Verification code - Client Connection |

---

**Total**: 8 EventHandlers × 2 langues = **16 templates à créer dans SharePoint**

---

**Auteur**: Équipe Technique AfDB
**Date**: 2025-01-06
**Version**: 1.0
