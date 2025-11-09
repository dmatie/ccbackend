# 📧 EXEMPLE TEMPLATE BILINGUE - ClaimCreated (Auteur)

Ce template contient **ANGLAIS en haut + FRANÇAIS en bas** dans un seul email.

---

## 📋 CONFIGURATION SHAREPOINT

| Champ | Valeur |
|-------|--------|
| **TemplateKey** | `ClaimCreated_Author` |
| **Language** | `bilingual` (ou juste laisser vide) |
| **Subject** | `Claim Submitted / Réclamation Soumise - #{{data.claimId}}` |

---

## 🔧 VARIABLES DISPONIBLES

```
{{recipientName}}
{{data.claimId}}
{{data.claimTypeEn}}
{{data.claimTypeFr}}
{{data.country}}
{{data.comment}}
{{data.createdDate}}
{{data.createdTime}}
```

---

## 📄 TEMPLATE HTML BILINGUE

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body{font-family:'Segoe UI',sans-serif;line-height:1.6;color:#333;margin:0;padding:0;background:#f5f5f5}
        .container{max-width:650px;margin:0 auto;padding:20px}

        /* Header bilingue */
        .header{background:linear-gradient(135deg,#667eea 0%,#764ba2 100%);color:white;padding:30px;text-align:center;border-radius:8px 8px 0 0}
        .header h1{margin:0;font-size:28px}
        .header p{margin:5px 0 0 0;opacity:0.9;font-size:16px}

        /* Séparateur de langue */
        .lang-separator{background:#e0e0e0;height:2px;margin:0;position:relative}
        .lang-separator span{position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);background:white;padding:5px 15px;font-size:12px;color:#666;font-weight:600;border:2px solid #e0e0e0;border-radius:20px}

        /* Section de contenu */
        .content-section{background:#ffffff;padding:30px;border-left:1px solid #e0e0e0;border-right:1px solid #e0e0e0}
        .content-section.english{border-top:1px solid #e0e0e0}
        .content-section.french{border-bottom:1px solid #e0e0e0}

        /* Flag indicator */
        .lang-flag{display:inline-block;padding:4px 12px;background:#667eea;color:white;border-radius:15px;font-size:12px;font-weight:600;margin-bottom:15px}

        /* Info boxes */
        .info-box{background:#f8f9fa;padding:20px;border-left:4px solid #667eea;margin:20px 0;border-radius:4px}
        .info-box h3{margin-top:0;color:#667eea;font-size:18px}
        .info-row{margin:10px 0}
        .label{font-weight:600;color:#555;display:inline-block;min-width:180px}
        .value{color:#333}

        /* Comment box */
        .comment-box{background:#fff3cd;padding:15px;border-left:4px solid #ffc107;margin:20px 0;border-radius:4px}
        .comment-box p{margin:0}
        .comment-box .comment-text{margin:10px 0 0 0;font-style:italic;line-height:1.8}

        /* Button */
        .button{display:inline-block;padding:12px 30px;background:#667eea;color:white;text-decoration:none;border-radius:5px;margin:20px 0}
        .button-container{text-align:center;margin-top:30px}

        /* Status badge */
        .status-badge{display:inline-block;padding:5px 15px;background:#28a745;color:white;border-radius:20px;font-size:14px}

        /* Footer */
        .footer{background:#f8f9fa;padding:20px;text-align:center;font-size:12px;color:#666;border-radius:0 0 8px 8px;border:1px solid #e0e0e0;border-top:none}

        /* Lists */
        .content-section ul{line-height:1.8;padding-left:20px}
        .content-section h4{color:#667eea;margin-top:30px;font-size:16px}
    </style>
</head>
<body>
    <div class="container">
        <!-- Header bilingue -->
        <div class="header">
            <h1>✅ Claim Submitted / Réclamation Soumise</h1>
            <p>Your claim has been registered / Votre réclamation a été enregistrée</p>
        </div>

        <!-- SECTION ANGLAISE -->
        <div class="content-section english">
            <div class="lang-flag">🇬🇧 ENGLISH</div>

            <p>Hello <strong>{{recipientName}}</strong>,</p>

            <p>We have received your claim. It is currently under review by our team.</p>

            <div class="info-box">
                <h3>📋 Claim Details</h3>
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
                    <span class="label">Submission date:</span>
                    <span class="value">{{data.createdDate}} at {{data.createdTime}}</span>
                </div>
                <div class="info-row">
                    <span class="label">Status:</span>
                    <span class="value"><span class="status-badge">Pending review</span></span>
                </div>
            </div>

            <div class="comment-box">
                <p><strong>📝 Your comment:</strong></p>
                <p class="comment-text">{{data.comment}}</p>
            </div>

            <h4>🔔 Next Steps</h4>
            <ul>
                <li>Our team will review your claim promptly</li>
                <li>You will receive a notification when action is taken</li>
                <li>You can track your claim progress in your dashboard</li>
            </ul>

            <div class="button-container">
                <a href="https://clientconnection.afdb.org/claims/{{data.claimId}}" class="button">View my claim</a>
            </div>
        </div>

        <!-- Séparateur -->
        <div class="lang-separator">
            <span>• • •</span>
        </div>

        <!-- SECTION FRANÇAISE -->
        <div class="content-section french">
            <div class="lang-flag">🇫🇷 FRANÇAIS</div>

            <p>Bonjour <strong>{{recipientName}}</strong>,</p>

            <p>Nous avons bien reçu votre réclamation. Elle est actuellement en cours d'examen par notre équipe.</p>

            <div class="info-box">
                <h3>📋 Détails de votre réclamation</h3>
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
                    <span class="value"><span class="status-badge">En attente de traitement</span></span>
                </div>
            </div>

            <div class="comment-box">
                <p><strong>📝 Votre commentaire :</strong></p>
                <p class="comment-text">{{data.comment}}</p>
            </div>

            <h4>🔔 Prochaines étapes</h4>
            <ul>
                <li>Notre équipe examinera votre réclamation dans les plus brefs délais</li>
                <li>Vous recevrez une notification dès qu'une action sera effectuée</li>
                <li>Vous pouvez suivre l'évolution de votre réclamation dans votre tableau de bord</li>
            </ul>

            <div class="button-container">
                <a href="https://clientconnection.afdb.org/claims/{{data.claimId}}" class="button">Voir ma réclamation</a>
            </div>
        </div>

        <!-- Footer bilingue -->
        <div class="footer">
            <p><strong>Client Connection</strong> | African Development Bank</p>
            <p>This is an automated email. Please do not reply. / Cet email a été envoyé automatiquement. Merci de ne pas y répondre.</p>
            <p style="margin-top:10px">For questions / Pour toute question : <a href="mailto:support@afdb.org">support@afdb.org</a></p>
        </div>
    </div>
</body>
</html>
```

---

## 🎨 DESIGN FEATURES

✅ **Header bilingue** avec les 2 langues
✅ **Séparateur visuel** entre EN et FR
✅ **Flags** 🇬🇧 et 🇫🇷 pour identifier chaque section
✅ **Même structure** pour les 2 langues (facile à lire)
✅ **Footer bilingue**
✅ **Un seul email** = plus simple pour Power Automate
✅ **Responsive** pour mobile

---

## 📊 AVANTAGES DE CETTE APPROCHE

1. **Plus simple pour SharePoint** - 1 seul template au lieu de 2
2. **Plus simple pour Power Automate** - Pas besoin de gérer la langue
3. **Utilisateur** peut lire dans sa langue préférée
4. **Maintenance** plus facile - Un seul template à maintenir
5. **Moins d'erreurs** - Pas de risque d'envoyer la mauvaise langue

---

## ✅ QU'EST-CE QUI CHANGE?

### Avant (2 templates séparés):
- ClaimCreated + Language="fr" → Email FR uniquement
- ClaimCreated + Language="en" → Email EN uniquement
- Power Automate doit choisir la langue

### Maintenant (1 template bilingue):
- ClaimCreated → Email EN + FR
- Power Automate envoie toujours le même template
- Utilisateur lit dans sa langue

---

## 🎯 CE TEMPLATE EST-IL BON POUR TOI?

Si oui, je vais:
1. Créer tous les templates bilingues pour les 8 EventHandlers
2. Supprimer la colonne "Language" de SharePoint (plus nécessaire)
3. Mettre à jour le guide d'implémentation

**Qu'en penses-tu? Veux-tu que je continue avec cette approche?** 🚀

---

**Note**: Le sujet de l'email est aussi bilingue:
```
Claim Submitted / Réclamation Soumise - #{{data.claimId}}
```
