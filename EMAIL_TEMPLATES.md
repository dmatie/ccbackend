# 📧 TEMPLATES D'EMAILS POUR TOUS LES EVENTHANDLERS

Ce document contient tous les templates d'emails HTML pour les EventHandlers migrés vers NotificationService.

---

## TABLE DES MATIÈRES

1. [ClaimCreated - Email 1 (Auteur - FR)](#1-claimcreated-email-1-auteur-fr)
2. [ClaimCreated - Email 2 (Assignés - EN)](#2-claimcreated-email-2-assignés-en)
3. [ClaimResponseAdded (FR)](#3-claimresponseadded-fr)
4. [DisbursementSubmitted (FR)](#4-disbursementsubmitted-fr)
5. [DisbursementReSubmitted (FR)](#5-disbursementresubmitted-fr)
6. [DisbursementBackedToClient (FR)](#6-disbursementbackedtoclient-fr)
7. [DisbursementRejected (FR)](#7-disbursementrejected-fr)
8. [DisbursementApproved (FR)](#8-disbursementapproved-fr)
9. [OtpCreated (FR)](#9-otpcreated-fr)

---

## 1. ClaimCreated - Email 1 (Auteur - FR)

### Sujet
```
Confirmation de soumission - Réclamation #{data.claimId}
```

### Variables disponibles
- `{{recipientName}}` - Nom du destinataire
- `{{data.claimId}}` - ID de la réclamation
- `{{data.claimTypeFr}}` - Type de réclamation en français
- `{{data.country}}` - Pays
- `{{data.createdDate}}` - Date (yyyy-MM-dd)
- `{{data.createdTime}}` - Heure (HH:mm)
- `{{data.comment}}` - Commentaire de l'utilisateur

### Template SharePoint
**TemplateKey**: `ClaimCreated`  
**Language**: `fr`  
**Target**: Auteur de la réclamation

### Corps du mail (HTML)
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }
        .container { max-width: 600px; margin: 0 auto; padding: 20px; }
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
                <a href="[URL_APP]/claims/{{data.claimId}}" class="button">Voir ma réclamation</a>
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

