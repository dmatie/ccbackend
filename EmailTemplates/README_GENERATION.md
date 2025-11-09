# Génération des Templates Email

## 📋 Templates à créer

Ce dossier doit contenir 9 templates HTML avec **styles 100% inline**:

1. `1_ClaimCreated_Author.html` - Confirmation réclamation (auteur)
2. `2_ClaimCreated_Assigned.html` - Notification assignation
3. `3_ClaimResponseAdded.html` - Réponse ajoutée
4. `4_DisbursementSubmitted.html` - Décaissement soumis
5. `5_DisbursementReSubmitted.html` - Décaissement resoumis
6. `6_DisbursementBackedToClient.html` - Retour au client
7. `7_DisbursementRejected.html` - Décaissement rejeté
8. `8_DisbursementApproved.html` - Décaissement approuvé
9. `9_OtpCreated.html` - Code OTP

## ✅ Caractéristiques

- **Styles 100% inline** (pas de `<style>` dans `<head>`)
- **Format TABLE HTML** pour compatibilité maximale
- **Bilingue** (Anglais en haut, Français en bas)
- **Compatible** avec Outlook, Gmail, tous clients email

## 🚀 Comment générer

Vu la taille des templates (100+ lignes chacun), vous avez 2 options:

### Option A: Script Python complet

Créez un fichier `generate_all.py` avec tous les templates en Python.

### Option B: Utiliser l'IA

Demandez à l'assistant de générer les 9 fichiers HTML directement
dans ce dossier avec les styles inline.

## 📝 Structure d'un template

Chaque template doit suivre cette structure:

\`\`\`html
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8"></head>
<body style="font-family:'Segoe UI',...">
<table width="100%" ...>
  <!-- Header avec gradient -->
  <!-- Section Anglaise -->
  <!-- Séparateur -->
  <!-- Section Française -->
  <!-- Footer -->
</table>
</body>
</html>
\`\`\`

## 🎨 Couleurs par template

1. ClaimCreated_Author: `#667eea` (violet)
2. ClaimCreated_Assigned: `#f5576c` (rose)
3. ClaimResponseAdded: `#4facfe` (bleu ciel)
4. DisbursementSubmitted: `#43e97b` (vert)
5. DisbursementReSubmitted: `#fa709a` (rose orangé)
6. DisbursementBackedToClient: `#ff9a56` (orange)
7. DisbursementRejected: `#eb3349` (rouge)
8. DisbursementApproved: `#11998e` (vert turquoise)
9. OtpCreated: `#667eea` (violet)

