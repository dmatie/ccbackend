# Templates 6, 7 et 8 - À ajouter dans ALL_EMAIL_TEMPLATES.md

Copier ces templates à la suite des templates 4 et 5.

---

# 6. DisbursementRejected

**EventHandler**: `DisbursementRejectedEventHandler`
**Emails envoyés**: 1 (Créateur)

## Variables disponibles
```
{{recipientName}}, {{data.disbursementId}}, {{data.requestNumber}}, {{data.sapCodeProject}},
{{data.loanGrantNumber}}, {{data.disbursementTypeCode}}, {{data.disbursementTypeName}},
{{data.rejectionComment}}, {{data.rejectedByFirstName}}, {{data.rejectedByLastName}},
{{data.rejectedByEmail}}, {{data.rejectedDate}}, {{data.rejectedTime}}
```

## 📧 Template FR

### Config SharePoint
- **TemplateKey**: `DisbursementRejected`
- **Language**: `fr`
- **Subject**: `Demande de décaissement rejetée - {{data.requestNumber}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#eb3349 0%,#f45c43 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #eb3349;margin:20px 0;border-radius:4px}
        .rejection-box{background:#ffebee;padding:20px;border-left:4px solid #d32f2f;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0;display:flex}
        .label{font-weight:600;color:#555;min-width:180px}
        .value{color:#333;flex:1}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#eb3349;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#d32f2f;color:white;border-radius:20px;font-size:14px;font-weight:600}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">❌ Décaissement Rejeté</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Votre demande n'a pas pu être approuvée</p>
        </div>
        <div class="content">
            <p>Bonjour <strong>{{recipientName}}</strong>,</p>
            <p>Nous regrettons de vous informer que votre demande de décaissement a été rejetée après examen.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#c62828">💰 Détails du décaissement</h3>
                <div class="info-row"><span class="label">Numéro de demande :</span><span class="value"><strong>{{data.requestNumber}}</strong></span></div>
                <div class="info-row"><span class="label">Type :</span><span class="value">{{data.disbursementTypeName}} ({{data.disbursementTypeCode}})</span></div>
                <div class="info-row"><span class="label">Code SAP Projet :</span><span class="value">{{data.sapCodeProject}}</span></div>
                <div class="info-row"><span class="label">Numéro Prêt/Don :</span><span class="value">{{data.loanGrantNumber}}</span></div>
                <div class="info-row"><span class="label">Rejeté par :</span><span class="value">{{data.rejectedByFirstName}} {{data.rejectedByLastName}}</span></div>
                <div class="info-row"><span class="label">Date de rejet :</span><span class="value">{{data.rejectedDate}} à {{data.rejectedTime}}</span></div>
                <div class="info-row"><span class="label">Statut :</span><span class="value"><span class="status-badge">Rejeté</span></span></div>
            </div>
            <div class="rejection-box">
                <h4 style="margin-top:0;color:#c62828">📝 Motif du rejet :</h4>
                <p style="margin:0;line-height:1.8;background:white;padding:15px;border-radius:4px">{{data.rejectionComment}}</p>
            </div>
            <h4 style="color:#c62828;margin-top:30px">🔄 Que faire ensuite ?</h4>
            <ul style="line-height:1.8">
                <li>Examinez attentivement les raisons du rejet ci-dessus</li>
                <li>Si vous pensez qu'il y a une erreur, contactez le responsable</li>
                <li>Si vous souhaitez soumettre une nouvelle demande, assurez-vous de corriger les problèmes mentionnés</li>
                <li>N'hésitez pas à demander des clarifications si nécessaire</li>
            </ul>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/disbursements/{{data.disbursementId}}" class="button">Voir les détails</a>
            </div>
        </div>
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>Pour toute question, contactez <a href="mailto:{{data.rejectedByEmail}}">{{data.rejectedByEmail}}</a></p>
        </div>
    </div>
</body>
</html>
```

## 📧 Template EN

### Config SharePoint
- **TemplateKey**: `DisbursementRejected`
- **Language**: `en`
- **Subject**: `Disbursement request rejected - {{data.requestNumber}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#eb3349 0%,#f45c43 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #eb3349;margin:20px 0;border-radius:4px}
        .rejection-box{background:#ffebee;padding:20px;border-left:4px solid #d32f2f;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0;display:flex}
        .label{font-weight:600;color:#555;min-width:180px}
        .value{color:#333;flex:1}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#eb3349;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#d32f2f;color:white;border-radius:20px;font-size:14px;font-weight:600}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">❌ Disbursement Rejected</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Your request could not be approved</p>
        </div>
        <div class="content">
            <p>Hello <strong>{{recipientName}}</strong>,</p>
            <p>We regret to inform you that your disbursement request has been rejected after review.</p>
            <div class="info-box">
                <h3 style="margin-top:0;color:#c62828">💰 Disbursement Details</h3>
                <div class="info-row"><span class="label">Request number:</span><span class="value"><strong>{{data.requestNumber}}</strong></span></div>
                <div class="info-row"><span class="label">Type:</span><span class="value">{{data.disbursementTypeName}} ({{data.disbursementTypeCode}})</span></div>
                <div class="info-row"><span class="label">SAP Project Code:</span><span class="value">{{data.sapCodeProject}}</span></div>
                <div class="info-row"><span class="label">Loan/Grant Number:</span><span class="value">{{data.loanGrantNumber}}</span></div>
                <div class="info-row"><span class="label">Rejected by:</span><span class="value">{{data.rejectedByFirstName}} {{data.rejectedByLastName}}</span></div>
                <div class="info-row"><span class="label">Rejection date:</span><span class="value">{{data.rejectedDate}} at {{data.rejectedTime}}</span></div>
                <div class="info-row"><span class="label">Status:</span><span class="value"><span class="status-badge">Rejected</span></span></div>
            </div>
            <div class="rejection-box">
                <h4 style="margin-top:0;color:#c62828">📝 Reason for rejection:</h4>
                <p style="margin:0;line-height:1.8;background:white;padding:15px;border-radius:4px">{{data.rejectionComment}}</p>
            </div>
            <h4 style="color:#c62828;margin-top:30px">🔄 What to do next?</h4>
            <ul style="line-height:1.8">
                <li>Carefully review the rejection reasons above</li>
                <li>If you believe there's an error, contact the manager</li>
                <li>If you wish to submit a new request, make sure to address the mentioned issues</li>
                <li>Don't hesitate to ask for clarifications if needed</li>
            </ul>
            <div style="text-align:center;margin-top:30px">
                <a href="https://clientconnection.afdb.org/disbursements/{{data.disbursementId}}" class="button">View details</a>
            </div>
        </div>
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>For questions, contact <a href="mailto:{{data.rejectedByEmail}}">{{data.rejectedByEmail}}</a></p>
        </div>
    </div>
</body>
</html>
```

---

# 7. DisbursementApproved

**EventHandler**: `DisbursementApprovedEventHandler`
**Emails envoyés**: 1 (Créateur)

## Variables disponibles
```
{{recipientName}}, {{data.disbursementId}}, {{data.requestNumber}}, {{data.sapCodeProject}},
{{data.loanGrantNumber}}, {{data.disbursementTypeCode}}, {{data.disbursementTypeName}},
{{data.approvedByFirstName}}, {{data.approvedByLastName}},
{{data.approvedDate}}, {{data.approvedTime}}
```

## 📧 Template FR

### Config SharePoint
- **TemplateKey**: `DisbursementApproved`
- **Language**: `fr`
- **Subject**: `✅ Demande de décaissement approuvée - {{data.requestNumber}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#11998e 0%,#38ef7d 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #11998e;margin:20px 0;border-radius:4px}
        .success-box{background:#e8f5e9;padding:20px;border-left:4px solid #4caf50;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0;display:flex}
        .label{font-weight:600;color:#555;min-width:180px}
        .value{color:#333;flex:1}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#11998e;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#4caf50;color:white;border-radius:20px;font-size:14px;font-weight:600}
        .celebration{font-size:48px;text-align:center;margin:20px 0}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <div class="celebration">🎉</div>
            <h1 style="margin:0;font-size:28px">Décaissement Approuvé</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Félicitations ! Votre demande a été approuvée</p>
        </div>
        <div class="content">
            <p>Bonjour <strong>{{recipientName}}</strong>,</p>
            <p>Excellente nouvelle ! Nous avons le plaisir de vous informer que votre demande de décaissement a été <strong>approuvée</strong>.</p>
            <div class="success-box">
                <h3 style="margin-top:0;color:#2e7d32;text-align:center">✅ DEMANDE APPROUVÉE</h3>
                <p style="text-align:center;margin:10px 0 0 0;font-size:18px;color:#1b5e20">Le traitement de votre décaissement va maintenant être effectué</p>
            </div>
            <div class="info-box">
                <h3 style="margin-top:0;color:#00695c">💰 Détails du décaissement</h3>
                <div class="info-row"><span class="label">Numéro de demande :</span><span class="value"><strong>{{data.requestNumber}}</strong></span></div>
                <div class="info-row"><span class="label">Type :</span><span class="value">{{data.disbursementTypeName}} ({{data.disbursementTypeCode}})</span></div>
                <div class="info-row"><span class="label">Code SAP Projet :</span><span class="value">{{data.sapCodeProject}}</span></div>
                <div class="info-row"><span class="label">Numéro Prêt/Don :</span><span class="value">{{data.loanGrantNumber}}</span></div>
                <div class="info-row"><span class="label">Approuvé par :</span><span class="value">{{data.approvedByFirstName}} {{data.approvedByLastName}}</span></div>
                <div class="info-row"><span class="label">Date d'approbation :</span><span class="value">{{data.approvedDate}} à {{data.approvedTime}}</span></div>
                <div class="info-row"><span class="label">Statut :</span><span class="value"><span class="status-badge">Approuvé</span></span></div>
            </div>
            <h4 style="color:#00695c;margin-top:30px">🔔 Prochaines étapes</h4>
            <ul style="line-height:1.8">
                <li>Le département financier va procéder au traitement de votre décaissement</li>
                <li>Les fonds seront transférés selon les modalités convenues</li>
                <li>Vous pouvez suivre le statut du traitement dans votre tableau de bord</li>
                <li>Une confirmation vous sera envoyée une fois le traitement finalisé</li>
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
- **TemplateKey**: `DisbursementApproved`
- **Language**: `en`
- **Subject**: `✅ Disbursement request approved - {{data.requestNumber}}`

### HTML Body
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0}
        .container{max-width:600px;margin:0 auto;padding:20px;background:#f5f5f5}
        .header{background:linear-gradient(135deg,#11998e 0%,#38ef7d 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .content{background:#ffffff;padding:30px;border:1px solid #e0e0e0;border-top:none}
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #11998e;margin:20px 0;border-radius:4px}
        .success-box{background:#e8f5e9;padding:20px;border-left:4px solid #4caf50;margin:20px 0;border-radius:4px}
        .info-row{margin:10px 0;display:flex}
        .label{font-weight:600;color:#555;min-width:180px}
        .value{color:#333;flex:1}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .button{display:inline-block;padding:12px 30px;background:#11998e;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .status-badge{display:inline-block;padding:5px 15px;background:#4caf50;color:white;border-radius:20px;font-size:14px;font-weight:600}
        .celebration{font-size:48px;text-align:center;margin:20px 0}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <div class="celebration">🎉</div>
            <h1 style="margin:0;font-size:28px">Disbursement Approved</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Congratulations! Your request has been approved</p>
        </div>
        <div class="content">
            <p>Hello <strong>{{recipientName}}</strong>,</p>
            <p>Great news! We are pleased to inform you that your disbursement request has been <strong>approved</strong>.</p>
            <div class="success-box">
                <h3 style="margin-top:0;color:#2e7d32;text-align:center">✅ REQUEST APPROVED</h3>
                <p style="text-align:center;margin:10px 0 0 0;font-size:18px;color:#1b5e20">The processing of your disbursement will now be carried out</p>
            </div>
            <div class="info-box">
                <h3 style="margin-top:0;color:#00695c">💰 Disbursement Details</h3>
                <div class="info-row"><span class="label">Request number:</span><span class="value"><strong>{{data.requestNumber}}</strong></span></div>
                <div class="info-row"><span class="label">Type:</span><span class="value">{{data.disbursementTypeName}} ({{data.disbursementTypeCode}})</span></div>
                <div class="info-row"><span class="label">SAP Project Code:</span><span class="value">{{data.sapCodeProject}}</span></div>
                <div class="info-row"><span class="label">Loan/Grant Number:</span><span class="value">{{data.loanGrantNumber}}</span></div>
                <div class="info-row"><span class="label">Approved by:</span><span class="value">{{data.approvedByFirstName}} {{data.approvedByLastName}}</span></div>
                <div class="info-row"><span class="label">Approval date:</span><span class="value">{{data.approvedDate}} at {{data.approvedTime}}</span></div>
                <div class="info-row"><span class="label">Status:</span><span class="value"><span class="status-badge">Approved</span></span></div>
            </div>
            <h4 style="color:#00695c;margin-top:30px">🔔 Next Steps</h4>
            <ul style="line-height:1.8">
                <li>The finance department will process your disbursement</li>
                <li>Funds will be transferred according to the agreed terms</li>
                <li>You can track the processing status in your dashboard</li>
                <li>A confirmation will be sent once processing is complete</li>
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

# 8. OtpCreated

**EventHandler**: `CreateOtpCommandHandler`
**Emails envoyés**: 1 (Utilisateur)

## Variables disponibles
```
{{recipientName}}, {{data.email}}, {{data.otpCode}}, {{data.expiresInMinutes}},
{{data.createdDate}}, {{data.createdTime}}
```

## 📧 Template FR

### Config SharePoint
- **TemplateKey**: `OtpCreated`
- **Language**: `fr`
- **Subject**: `Code de vérification - Client Connection`

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
        .otp-box{background:linear-gradient(135deg,#667eea 0%,#764ba2 100%);padding:30px;margin:30px 0;border-radius:8px;text-align:center;box-shadow:0 4px 15px rgba(102,126,234,0.3)}
        .otp-code{font-size:52px;font-weight:bold;color:white;letter-spacing:12px;margin:20px 0;font-family:'Courier New',monospace;text-shadow:2px 2px 4px rgba(0,0,0,0.2)}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .timer{font-size:24px;color:white;margin-top:10px}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">🔐 Code de Vérification</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Authentification Client Connection</p>
        </div>
        <div class="content">
            <p>Bonjour,</p>
            <p>Vous avez demandé un code de vérification pour accéder à <strong>Client Connection</strong>.</p>
            <div class="otp-box">
                <p style="color:white;margin:0;font-size:16px;opacity:0.9">VOTRE CODE DE VÉRIFICATION</p>
                <div class="otp-code">{{data.otpCode}}</div>
                <div class="timer">⏰ Valide pendant {{data.expiresInMinutes}} minutes</div>
            </div>
            <h4 style="color:#667eea;margin-top:30px">📝 Comment utiliser ce code ?</h4>
            <ol style="line-height:1.8">
                <li>Copiez le code à 6 chiffres ci-dessus</li>
                <li>Retournez à la page d'authentification</li>
                <li>Collez ou tapez le code dans le champ prévu</li>
                <li>Cliquez sur "Vérifier" pour continuer</li>
            </ol>
            <div style="background:#fff3cd;padding:15px;border-left:4px solid #ff9800;margin:20px 0;border-radius:4px">
                <p style="margin:0"><strong>🔒 Sécurité Important :</strong></p>
                <ul style="margin:10px 0 0 20px;padding:0">
                    <li>Ne partagez <strong>JAMAIS</strong> ce code avec qui que ce soit</li>
                    <li>L'équipe AfDB ne vous demandera jamais ce code</li>
                    <li>Ce code expire automatiquement après {{data.expiresInMinutes}} minutes</li>
                </ul>
            </div>
            <div style="background:#ffebee;padding:15px;border-left:4px solid #f44336;margin:20px 0;border-radius:4px">
                <p style="margin:0"><strong>⚠️ Vous n'avez pas demandé ce code ?</strong></p>
                <p style="margin:10px 0 0 0">Ignorez simplement cet email. Le code expirera automatiquement.</p>
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
- **TemplateKey**: `OtpCreated`
- **Language**: `en`
- **Subject**: `Verification code - Client Connection`

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
        .otp-box{background:linear-gradient(135deg,#667eea 0%,#764ba2 100%);padding:30px;margin:30px 0;border-radius:8px;text-align:center;box-shadow:0 4px 15px rgba(102,126,234,0.3)}
        .otp-code{font-size:52px;font-weight:bold;color:white;letter-spacing:12px;margin:20px 0;font-family:'Courier New',monospace;text-shadow:2px 2px 4px rgba(0,0,0,0.2)}
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px}
        .timer{font-size:24px;color:white;margin-top:10px}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1 style="margin:0;font-size:28px">🔐 Verification Code</h1>
            <p style="margin:10px 0 0 0;opacity:0.9">Client Connection Authentication</p>
        </div>
        <div class="content">
            <p>Hello,</p>
            <p>You have requested a verification code to access <strong>Client Connection</strong>.</p>
            <div class="otp-box">
                <p style="color:white;margin:0;font-size:16px;opacity:0.9">YOUR VERIFICATION CODE</p>
                <div class="otp-code">{{data.otpCode}}</div>
                <div class="timer">⏰ Valid for {{data.expiresInMinutes}} minutes</div>
            </div>
            <h4 style="color:#667eea;margin-top:30px">📝 How to use this code?</h4>
            <ol style="line-height:1.8">
                <li>Copy the 6-digit code above</li>
                <li>Return to the authentication page</li>
                <li>Paste or type the code in the provided field</li>
                <li>Click "Verify" to continue</li>
            </ol>
            <div style="background:#fff3cd;padding:15px;border-left:4px solid #ff9800;margin:20px 0;border-radius:4px">
                <p style="margin:0"><strong>🔒 Important Security:</strong></p>
                <ul style="margin:10px 0 0 20px;padding:0">
                    <li><strong>NEVER</strong> share this code with anyone</li>
                    <li>The AfDB team will never ask for this code</li>
                    <li>This code expires automatically after {{data.expiresInMinutes}} minutes</li>
                </ul>
            </div>
            <div style="background:#ffebee;padding:15px;border-left:4px solid #f44336;margin:20px 0;border-radius:4px">
                <p style="margin:0"><strong>⚠️ Didn't request this code?</strong></p>
                <p style="margin:10px 0 0 0">Simply ignore this email. The code will expire automatically.</p>
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

**FIN DES TEMPLATES - Tous les 8 EventHandlers sont maintenant complets!**
