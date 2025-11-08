# Templates 4 à 8 - À ajouter dans ALL_EMAIL_TEMPLATES.md

Copier ces templates dans ALL_EMAIL_TEMPLATES.md avant le tableau récapitulatif (ligne 475).

---

# 4. DisbursementReSubmitted

**EventHandler**: `DisbursementReSubmittedEventHandler`
**Emails envoyés**: 1 (Créateur)

## Variables disponibles
```
{{recipientName}}, {{data.disbursementId}}, {{data.requestNumber}}, {{data.sapCodeProject}},
{{data.loanGrantNumber}}, {{data.disbursementTypeCode}}, {{data.disbursementTypeName}},
{{data.comment}}, {{data.resubmittedDate}}, {{data.resubmittedTime}}
```

## 📧 Template FR

### Config SharePoint
- **TemplateKey**: `DisbursementReSubmitted`
- **Language**: `fr`
- **Subject**: `Demande de décaissement resoumise - {{data.requestNumber}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#fa709a 0%,#fee140 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #fa709a;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0;display:flex}
        .label{font-weight:600;color:#555;min-width:180px}
        .value{color:#333;flex:1}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#fa709a;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#ff9800;color:white;border-radius:20px;font-size:14px;font-weight:600}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">🔄 Décaissement Resoumis</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Votre demande modifiée a été resoumise</p>
        </div>
        <div class="content">
            <p>Bonjour <strong>{{recipientName}}</strong>,</p>
            <p>Nous confirmons la resoumission de votre demande de décaissement après modifications. Elle est à nouveau en cours d'examen.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#d84315">💰 Détails du décaissement</h3>
                <div class="info-row"><span class="label">Numéro de demande :</span><span class="value"><strong>{{data.requestNumber}}</strong></span></div>
                <div class="info-row"><span class="label">Type :</span><span class="value">{{data.disbursementTypeName}} ({{data.disbursementTypeCode}})</span></div>
                <div class="info-row"><span class="label">Code SAP Projet :</span><span class="value">{{data.sapCodeProject}}</span></div>
                <div class="info-row"><span class="label">Numéro Prêt/Don :</span><span class="value">{{data.loanGrantNumber}}</span></div>
                <div class="info-row"><span class="label">Date de resoumission :</span><span class="value">{{data.resubmittedDate}} à {{data.resubmittedTime}}</span></div>
                <div class="info-row"><span class="label">Statut :</span><span class="value"><span class="status-badge">En attente d'approbation</span></span></div>
            </div>
            <div style="background:#fff3cd;padding:15px;border-left:4px solid #ffc107;margin:20px 0;border-radius:4px">
                <p style="margin:0"><strong>📝 Votre commentaire de resoumission :</strong></p>
                <p style="margin:10px 0 0 0;line-height:1.8">{{data.comment}}</p>
            </div>
            <h4 style="color:#d84315;margin-top:30px">🔔 Prochaines étapes</h4>
            <ul style="line-height:1.8">
                <li>Votre demande modifiée sera réexaminée</li>
                <li>Nous vérifierons que toutes les remarques ont été prises en compte</li>
                <li>Vous recevrez une notification de la décision finale</li>
            </ul>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/disbursements/{{data.disbursementId}}" class="button">Voir ma demande</a>
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
- **TemplateKey**: `DisbursementReSubmitted`
- **Language**: `en`
- **Subject**: `Disbursement request resubmitted - {{data.requestNumber}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#fa709a 0%,#fee140 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #fa709a;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0;display:flex}
        .label{font-weight:600;color:#555;min-width:180px}
        .value{color:#333;flex:1}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#fa709a;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#ff9800;color:white;border-radius:20px;font-size:14px;font-weight:600}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">🔄 Disbursement Resubmitted</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Your modified request has been resubmitted</p>
        </div>
        <div class="content">
            <p>Hello <strong>{{recipientName}}</strong>,</p>
            <p>We confirm the resubmission of your disbursement request after modifications. It is under review again.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#d84315">💰 Disbursement Details</h3>
                <div class="info-row"><span class="label">Request number:</span><span class="value"><strong>{{data.requestNumber}}</strong></span></div>
                <div class="info-row"><span class="label">Type:</span><span class="value">{{data.disbursementTypeName}} ({{data.disbursementTypeCode}})</span></div>
                <div class="info-row"><span class="label">SAP Project Code:</span><span class="value">{{data.sapCodeProject}}</span></div>
                <div class="info-row"><span class="label">Loan/Grant Number:</span><span class="value">{{data.loanGrantNumber}}</span></div>
                <div class="info-row"><span class="label">Resubmission date:</span><span class="value">{{data.resubmittedDate}} at {{data.resubmittedTime}}</span></div>
                <div class="info-row"><span class="label">Status:</span><span class="value"><span class="status-badge">Pending approval</span></span></div>
            </div>
            <div style="background:#fff3cd;padding:15px;border-left:4px solid #ffc107;margin:20px 0;border-radius:4px">
                <p style="margin:0"><strong>📝 Your resubmission comment:</strong></p>
                <p style="margin:10px 0 0 0;line-height:1.8">{{data.comment}}</p>
            </div>
            <h4 style="color:#d84315;margin-top:30px">🔔 Next Steps</h4>
            <ul style="line-height:1.8">
                <li>Your modified request will be reviewed again</li>
                <li>We will verify that all comments have been addressed</li>
                <li>You will receive a notification with the final decision</li>
            </ul>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/disbursements/{{data.disbursementId}}" class="button">View my request</a>
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

# 5. DisbursementBackedToClient

**EventHandler**: `DisbursementBackedToClientEventHandler`
**Emails envoyés**: 1 (Créateur)

## Variables disponibles
```
{{recipientName}}, {{data.disbursementId}}, {{data.requestNumber}}, {{data.sapCodeProject}},
{{data.loanGrantNumber}}, {{data.disbursementTypeCode}}, {{data.disbursementTypeName}},
{{data.comment}}, {{data.processedByFirstName}}, {{data.processedByLastName}},
{{data.processedByEmail}}, {{data.backedDate}}, {{data.backedTime}}
```

## 📧 Template FR

### Config SharePoint
- **TemplateKey**: `DisbursementBackedToClient`
- **Language**: `fr`
- **Subject**: `Action requise - Modifications demandées sur {{data.requestNumber}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#ff9a56 0%,#ff6a88 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #ff9a56;margin:20px 0;border-radius:4px}
        .warning-box{background:#fff3cd;padding:20px;border-left:4px solid #ff9800;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0;display:flex}
        .label{font-weight:600;color:#555;min-width:180px}
        .value{color:#333;flex:1}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#ff9a56;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#ff9800;color:white;border-radius:20px;font-size:14px;font-weight:600}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">⚠️ Action Requise</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Modifications nécessaires sur votre décaissement</p>
        </div>
        <div class="content">
            <p>Bonjour <strong>{{recipientName}}</strong>,</p>
            <p>Après examen de votre demande de décaissement, des modifications sont nécessaires avant que nous puissions procéder à l'approbation.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#e65100">💰 Détails du décaissement</h3>
                <div class="info-row"><span class="label">Numéro de demande :</span><span class="value"><strong>{{data.requestNumber}}</strong></span></div>
                <div class="info-row"><span class="label">Type :</span><span class="value">{{data.disbursementTypeName}} ({{data.disbursementTypeCode}})</span></div>
                <div class="info-row"><span class="label">Code SAP Projet :</span><span class="value">{{data.sapCodeProject}}</span></div>
                <div class="info-row"><span class="label">Numéro Prêt/Don :</span><span class="value">{{data.loanGrantNumber}}</span></div>
                <div class="info-row"><span class="label">Examiné par :</span><span class="value">{{data.processedByFirstName}} {{data.processedByLastName}}</span></div>
                <div class="info-row"><span class="label">Date d'examen :</span><span class="value">{{data.backedDate}} à {{data.backedTime}}</span></div>
                <div class="info-row"><span class="label">Statut :</span><span class="value"><span class="status-badge">Modifications requises</span></span></div>
            </div>
            <div class="warning-box">
                <h4 style="margin-top:0;color:#e65100">📝 Commentaires et modifications demandées :</h4>
                <p style="margin:0;line-height:1.8;background:white;padding:15px;border-radius:4px">{{data.comment}}</p>
            </div>
            <h4 style="color:#e65100;margin-top:30px">✅ Actions à effectuer</h4>
            <ol style="line-height:1.8">
                <li>Lisez attentivement les commentaires ci-dessus</li>
                <li>Effectuez les modifications demandées sur votre demande</li>
                <li>Ajoutez ou mettez à jour les documents si nécessaire</li>
                <li>Resoumettez votre demande pour un nouvel examen</li>
            </ol>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/disbursements/{{data.disbursementId}}/edit" class="button">Modifier ma demande</a>
            </div>
        </div>
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>Pour toute question, contactez <a href="mailto:{{data.processedByEmail}}">{{data.processedByEmail}}</a></p>
        </div>
    </div>
</body>
</html>
```

## 📧 Template EN

### Config SharePoint
- **TemplateKey**: `DisbursementBackedToClient`
- **Language**: `en`
- **Subject**: `Action required - Changes requested on {{data.requestNumber}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#ff9a56 0%,#ff6a88 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #ff9a56;margin:20px 0;border-radius:4px}
        .warning-box{background:#fff3cd;padding:20px;border-left:4px solid #ff9800;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0;display:flex}
        .label{font-weight:600;color:#555;min-width:180px}
        .value{color:#333;flex:1}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#ff9a56;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#ff9800;color:white;border-radius:20px;font-size:14px;font-weight:600}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">⚠️ Action Required</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Changes needed on your disbursement</p>
        </div>
        <div class="content">
            <p>Hello <strong>{{recipientName}}</strong>,</p>
            <p>After reviewing your disbursement request, changes are needed before we can proceed with approval.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#e65100">💰 Disbursement Details</h3>
                <div class="info-row"><span class="label">Request number:</span><span class="value"><strong>{{data.requestNumber}}</strong></span></div>
                <div class="info-row"><span class="label">Type:</span><span class="value">{{data.disbursementTypeName}} ({{data.disbursementTypeCode}})</span></div>
                <div class="info-row"><span class="label">SAP Project Code:</span><span class="value">{{data.sapCodeProject}}</span></div>
                <div class="info-row"><span class="label">Loan/Grant Number:</span><span class="value">{{data.loanGrantNumber}}</span></div>
                <div class="info-row"><span class="label">Reviewed by:</span><span class="value">{{data.processedByFirstName}} {{data.processedByLastName}}</span></div>
                <div class="info-row"><span class="label">Review date:</span><span class="value">{{data.backedDate}} at {{data.backedTime}}</span></div>
                <div class="info-row"><span class="label">Status:</span><span class="value"><span class="status-badge">Changes required</span></span></div>
            </div>
            <div class="warning-box">
                <h4 style="margin-top:0;color:#e65100">📝 Comments and requested changes:</h4>
                <p style="margin:0;line-height:1.8;background:white;padding:15px;border-radius:4px">{{data.comment}}</p>
            </div>
            <h4 style="color:#e65100;margin-top:30px">✅ Actions to take</h4>
            <ol style="line-height:1.8">
                <li>Read the comments above carefully</li>
                <li>Make the requested changes to your request</li>
                <li>Add or update documents if necessary</li>
                <li>Resubmit your request for a new review</li>
            </ol>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/disbursements/{{data.disbursementId}}/edit" class="button">Edit my request</a>
            </div>
        </div>
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>For questions, contact <a href="mailto:{{data.processedByEmail}}">{{data.processedByEmail}}</a></p>
        </div>
    </div>
</body>
</html>
```

---

**CONTINUEZ AVEC LES TEMPLATES 6, 7 ET 8 DANS LE PROCHAIN FICHIER**
