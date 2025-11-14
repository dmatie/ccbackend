# 🐳 Guide d'Exécution Locale avec Docker Desktop

Guide complet pour exécuter le backend AfDB Client Connection sur Docker Desktop en local.

---

## 📋 Table des Matières

1. [Prérequis](#prérequis)
2. [Option 1: Docker Compose (Recommandé)](#option-1-docker-compose-recommandé)
3. [Option 2: Docker Simple](#option-2-docker-simple)
4. [Option 3: Visual Studio/Rider](#option-3-visual-studiorider)
5. [Troubleshooting](#troubleshooting)

---

## 🔧 Prérequis

### Logiciels Requis

- ✅ **Docker Desktop** installé et démarré
  ```bash
  docker --version  # >= 20.10
  docker-compose --version  # >= 2.0
  ```

- ✅ **.NET 9.0 SDK** (pour build local)
  ```bash
  dotnet --version  # >= 9.0
  ```

### Services Optionnels

Pour un environnement complet, tu peux avoir besoin de:

- Azure Key Vault (ou mock local)
- Service Bus (ou utiliser les mocks)
- SharePoint (ou utiliser stockage local)

**Note**: Le backend peut fonctionner avec les mocks activés pour le développement!

---

## 🚀 Option 1: Docker Compose (Recommandé)

### Étape 1: Démarrer SQL Server

```bash
# Démarrer SQL Server en background
cd Dependencies
docker-compose -f docker-compose.dev.yml up -d

# Vérifier que SQL Server est prêt
docker logs sqlserver_dev

# Tu devrais voir: "SQL Server is now ready for client connections"
```

**Credentials SQL Server:**
- Host: `localhost:5001`
- User: `sa`
- Password: `aDb#Cc@Pwd!dev`

### Étape 2: Créer le docker-compose pour l'API

Je vais créer un nouveau fichier docker-compose complet:

```yaml
# Créer: Dependencies/docker-compose.full.yml
version: '3.9'
name: afdb-cc-local

services:
  # SQL Server
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: afdb_sqlserver
    restart: unless-stopped
    environment:
      SA_PASSWORD: "aDb#Cc@Pwd!dev"
      ACCEPT_EULA: "Y"
    ports:
      - "5001:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    networks:
      - afdb_net
    healthcheck:
      test: /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'aDb#Cc@Pwd!dev' -Q 'SELECT 1'
      interval: 10s
      timeout: 5s
      retries: 5

  # API Backend
  api:
    build:
      context: ..
      dockerfile: src/Afdb.ClientConnection.Api/Dockerfile
    container_name: afdb_api
    restart: unless-stopped
    ports:
      - "5000:8080"   # HTTP
      - "5001:8081"   # HTTPS (si configuré)
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:8080

      # Database
      - ConnectionStrings__DbConnectionString=Server=sqlserver,1433;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;

      # Azure AD (utilise tes valeurs ou mocks)
      - AzureAd__Instance=https://login.microsoftonline.com/
      - AzureAd__TenantId=515af266-ba4c-4452-a190-d3a7520a6957
      - AzureAd__ClientId=586ffe0a-59c4-4e52-af81-be102e2e6e07
      - AzureAd__Audience=api://586ffe0a-59c4-4e52-af81-be102e2e6e07
      - AzureAd__Domain=devmouslimattoutlook.onmicrosoft.com

      # Key Vault (désactivé en local)
      - KeyVault__VaultUri=

      # CORS
      - Cors__AllowedOrigins__0=http://localhost:4200
      - Cors__AllowedOrigins__1=http://localhost:3000

      # Mocks activés
      - Graph__UseMock=true
      - Sap__UseMock=true
      - SharePoint__UseSharePointStorage=false

    depends_on:
      sqlserver:
        condition: service_healthy
    networks:
      - afdb_net
    healthcheck:
      test: wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1
      interval: 30s
      timeout: 5s
      retries: 3

volumes:
  sqlserver_data:

networks:
  afdb_net:
    driver: bridge
```

### Étape 3: Démarrer tout ensemble

```bash
# Depuis la racine du projet
cd Dependencies

# Démarrer SQL Server + API
docker-compose -f docker-compose.full.yml up -d

# Voir les logs
docker-compose -f docker-compose.full.yml logs -f

# Attendre que l'API soit prête (1-2 minutes)
# Tu devrais voir: "Now listening on: http://[::]:8080"
```

### Étape 4: Appliquer les migrations

```bash
# Option A: Depuis l'hôte (si .NET installé)
cd src/Afdb.ClientConnection.Infrastructure

dotnet ef database update \
  --startup-project ../Afdb.ClientConnection.Api \
  --connection "Server=localhost,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;"

# Option B: Exec dans le container
docker exec -it afdb_api dotnet ef database update
```

### Étape 5: Tester l'API

```bash
# Health check
curl http://localhost:5000/health
# Réponse attendue: "Healthy"

# Swagger UI
open http://localhost:5000/swagger

# Tester un endpoint (sans auth pour test)
curl http://localhost:5000/api/references/countries
```

### Arrêter les services

```bash
# Arrêter tout
docker-compose -f docker-compose.full.yml down

# Arrêter et supprimer les volumes (⚠️ perte de données)
docker-compose -f docker-compose.full.yml down -v
```

---

## 🔨 Option 2: Docker Simple (API seulement)

Si tu veux juste tester le container de l'API:

### Étape 1: Build l'image

```bash
# Depuis la racine du projet
docker build \
  -f src/Afdb.ClientConnection.Api/Dockerfile \
  -t afdb-cc-api:local \
  .

# Vérifier l'image
docker images | grep afdb-cc-api
```

### Étape 2: Démarrer SQL Server séparément

```bash
cd Dependencies
docker-compose -f docker-compose.dev.yml up -d
```

### Étape 3: Run l'API

```bash
docker run -d \
  --name afdb-api \
  -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e "ConnectionStrings__DbConnectionString=Server=host.docker.internal,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;" \
  -e "Graph__UseMock=true" \
  -e "Sap__UseMock=true" \
  -e "SharePoint__UseSharePointStorage=false" \
  -e "Cors__AllowedOrigins__0=http://localhost:4200" \
  afdb-cc-api:local

# Voir les logs
docker logs -f afdb-api

# Health check
curl http://localhost:5000/health
```

**Note**: `host.docker.internal` permet au container de se connecter à localhost de ton hôte.

### Arrêter

```bash
docker stop afdb-api
docker rm afdb-api
```

---

## 💻 Option 3: Visual Studio / Rider

### Visual Studio

1. **Ouvrir le projet**
   - Ouvre `Afdb.ClientConnection.sln`

2. **Configurer le profil Docker**
   - Le profil "Container (Dockerfile)" existe déjà dans `launchSettings.json`

3. **Démarrer SQL Server**
   ```bash
   cd Dependencies
   docker-compose -f docker-compose.dev.yml up -d
   ```

4. **Configurer appsettings.Development.json**

   Crée/modifie: `src/Afdb.ClientConnection.Api/appsettings.Development.json`

   ```json
   {
     "ConnectionStrings": {
       "DbConnectionString": "Server=localhost,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;"
     },
     "Graph": {
       "UseMock": true
     },
     "Sap": {
       "UseMock": true
     },
     "SharePoint": {
       "UseSharePointStorage": false
     },
     "KeyVault": {
       "VaultUri": ""
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     }
   }
   ```

5. **Run avec Docker**
   - Sélectionne le profil "Container (Dockerfile)"
   - Appuie sur F5 ou "Start Debugging"
   - Visual Studio build et run automatiquement le container

6. **Appliquer les migrations**
   - Package Manager Console:
     ```powershell
     Update-Database
     ```

### JetBrains Rider

1. **Ouvrir le projet**
   - Ouvre `Afdb.ClientConnection.sln`

2. **Ajouter Docker Run Configuration**
   - Run > Edit Configurations
   - Add New > Docker > Docker Image
   - Dockerfile: `src/Afdb.ClientConnection.Api/Dockerfile`
   - Context folder: racine du projet
   - Container name: `afdb-api`
   - Ports: `5000:8080`

3. **Ajouter Environment Variables**
   ```
   ASPNETCORE_ENVIRONMENT=Development
   ConnectionStrings__DbConnectionString=Server=host.docker.internal,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;
   Graph__UseMock=true
   Sap__UseMock=true
   ```

4. **Run**
   - Sélectionne la configuration Docker
   - Clique sur Run

---

## 🛠️ Configuration Complète

### Fichier .env Local (Optionnel)

Crée un fichier `.env.local` à la racine:

```bash
# Database
DB_CONNECTION_STRING=Server=localhost,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;

# Azure AD
AZURE_AD_TENANT_ID=515af266-ba4c-4452-a190-d3a7520a6957
AZURE_AD_CLIENT_ID=586ffe0a-59c4-4e52-af81-be102e2e6e07
AZURE_AD_DOMAIN=devmouslimattoutlook.onmicrosoft.com

# CORS
CORS_ALLOWED_ORIGINS=http://localhost:4200,http://localhost:3000

# Mocks
USE_MOCKS=true
```

### appsettings.Development.json Complet

```json
{
  "ConnectionStrings": {
    "DbConnectionString": "Server=localhost,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;",
    "ServiceBusConnectionString": ""
  },
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "devmouslimattoutlook.onmicrosoft.com",
    "TenantId": "515af266-ba4c-4452-a190-d3a7520a6957",
    "ClientId": "586ffe0a-59c4-4e52-af81-be102e2e6e07",
    "Audience": "api://586ffe0a-59c4-4e52-af81-be102e2e6e07"
  },
  "Graph": {
    "UseMock": true,
    "MockAdminUsers": [ "O.SORO@AFDB.ORG", "M.DIARRASSOUBA@AFDB.ORG" ],
    "MockFifc3DOUsers": [ "O.SORO@AFDB.ORG" ],
    "MockFifc3DAUsers": [ "M.DIARRASSOUBA@AFDB.ORG" ]
  },
  "KeyVault": {
    "VaultUri": ""
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:4200",
      "http://localhost:3000",
      "http://localhost:5173"
    ]
  },
  "ServiceBus": {
    "AccessRequestQueue": "registration",
    "AccessRequestResponseQueue": "registration-response"
  },
  "SharePoint": {
    "UseSharePointStorage": false
  },
  "Sap": {
    "UseMock": true
  },
  "PowerAutomate": {
    "ClaimCreatedUrl": "",
    "ClaimResponseUrl": "",
    "OtpSendUrl": "",
    "NotificationFlowUrl": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

---

## 🔍 Troubleshooting

### Problème 1: SQL Server ne démarre pas

**Symptôme**: Container sqlserver_dev crash ou redémarre

**Solution**:
```bash
# Voir les logs
docker logs sqlserver_dev

# Possible: Pas assez de RAM
# Docker Desktop > Settings > Resources > Memory: augmenter à 4GB minimum

# Ou: Port 5001 déjà utilisé
docker-compose -f docker-compose.dev.yml down
netstat -ano | grep 5001  # Windows
lsof -i :5001             # Mac/Linux
```

### Problème 2: API ne peut pas se connecter à SQL Server

**Symptôme**: `A network-related or instance-specific error`

**Solutions**:

1. **Vérifier que SQL Server est prêt**
   ```bash
   docker exec -it sqlserver_dev /opt/mssql-tools/bin/sqlcmd \
     -S localhost -U sa -P 'aDb#Cc@Pwd!dev' -Q 'SELECT 1'
   ```

2. **Vérifier la connection string**
   - Depuis container API vers SQL container: `Server=sqlserver,1433`
   - Depuis hôte vers SQL container: `Server=localhost,5001`
   - Note: `TrustServerCertificate=True` est requis pour dev

3. **Vérifier le réseau Docker**
   ```bash
   docker network ls
   docker network inspect afdb_net  # ou dev_net
   ```

### Problème 3: Migrations échouent

**Symptôme**: `Cannot apply migrations`

**Solutions**:

1. **Vérifier la connection DB**
   ```bash
   # Test connection
   docker exec -it sqlserver_dev /opt/mssql-tools/bin/sqlcmd \
     -S localhost -U sa -P 'aDb#Cc@Pwd!dev' -Q 'SELECT @@VERSION'
   ```

2. **Appliquer manuellement**
   ```bash
   cd src/Afdb.ClientConnection.Infrastructure

   dotnet ef database update \
     --startup-project ../Afdb.ClientConnection.Api \
     --connection "Server=localhost,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;" \
     --verbose
   ```

3. **Créer la DB manuellement**
   ```bash
   docker exec -it sqlserver_dev /opt/mssql-tools/bin/sqlcmd \
     -S localhost -U sa -P 'aDb#Cc@Pwd!dev' \
     -Q 'CREATE DATABASE ClientConnection'
   ```

### Problème 4: Port 5000 déjà utilisé

**Symptôme**: `Address already in use`

**Solutions**:

1. **Changer le port**
   ```bash
   # Utiliser 5050 au lieu de 5000
   docker run -p 5050:8080 ...
   ```

2. **Trouver qui utilise le port**
   ```bash
   # Windows
   netstat -ano | findstr :5000

   # Mac/Linux
   lsof -i :5000
   ```

### Problème 5: "Swagger not found" en production

**Symptôme**: 404 sur `/swagger`

**Explication**: C'est normal! Swagger est désactivé en production par sécurité.

**Solution**: Utilise `ASPNETCORE_ENVIRONMENT=Development`

### Problème 6: Key Vault errors

**Symptôme**: `Azure.Identity.AuthenticationFailedException`

**Solution**: En local, désactive Key Vault
```json
{
  "KeyVault": {
    "VaultUri": ""
  }
}
```

Le code dans `Program.cs` ligne 11-15 ne tentera pas de se connecter si `VaultUri` est vide.

---

## 📝 Checklist de Démarrage Rapide

### Premier Démarrage

- [ ] Docker Desktop est démarré
- [ ] Cloner le repo
- [ ] Créer `appsettings.Development.json` (voir ci-dessus)
- [ ] Démarrer SQL Server: `docker-compose -f Dependencies/docker-compose.dev.yml up -d`
- [ ] Attendre 30 secondes
- [ ] Appliquer migrations: `dotnet ef database update`
- [ ] Build image API: `docker build -f src/Afdb.ClientConnection.Api/Dockerfile -t afdb-api .`
- [ ] Run API: voir commandes ci-dessus
- [ ] Test: `curl http://localhost:5000/health`
- [ ] Swagger: `http://localhost:5000/swagger`

---

## 🎯 Commandes Utiles

```bash
# Voir tous les containers
docker ps -a

# Voir tous les networks
docker network ls

# Voir tous les volumes
docker volume ls

# Nettoyer tout (⚠️ supprime données)
docker system prune -a --volumes

# Logs en temps réel
docker logs -f afdb_api

# Exec dans le container
docker exec -it afdb_api bash

# Vérifier la santé
docker inspect --format='{{.State.Health.Status}}' afdb_api

# Stats temps réel
docker stats afdb_api

# Rebuild sans cache
docker build --no-cache -f src/Afdb.ClientConnection.Api/Dockerfile -t afdb-api .
```

---

## 📚 Ressources

- [Docker Desktop Documentation](https://docs.docker.com/desktop/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet)
- [SQL Server Docker Images](https://hub.docker.com/_/microsoft-mssql-server)

---

**Créé le**: 2025-11-13
**Version**: 1.0
