#!/bin/bash

# Script de démarrage rapide pour Docker Desktop
# Usage: ./start-local.sh

set -e

echo "🚀 Démarrage de l'environnement AfDB Client Connection local..."
echo ""

# Couleurs
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Vérifier que Docker est démarré
if ! docker info > /dev/null 2>&1; then
    echo -e "${RED}❌ Docker n'est pas démarré. Démarre Docker Desktop et réessaye.${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Docker est démarré${NC}"
echo ""

# Nettoyer les anciens containers si nécessaire
echo "🧹 Nettoyage des anciens containers..."
docker-compose -f Dependencies/docker-compose.full.yml down 2>/dev/null || true
echo ""

# Démarrer SQL Server d'abord
echo "📦 Démarrage de SQL Server..."
cd Dependencies
docker-compose -f docker-compose.full.yml up -d sqlserver
cd ..

# Attendre que SQL Server soit prêt
echo "⏳ Attente que SQL Server soit prêt (cela peut prendre 30-60 secondes)..."
for i in {1..30}; do
    if docker exec afdb_sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'aDb#Cc@Pwd!dev' -Q 'SELECT 1' > /dev/null 2>&1; then
        echo -e "${GREEN}✅ SQL Server est prêt!${NC}"
        break
    fi
    echo -n "."
    sleep 2
    if [ $i -eq 30 ]; then
        echo -e "${RED}❌ SQL Server n'a pas démarré à temps${NC}"
        echo "Logs SQL Server:"
        docker logs afdb_sqlserver
        exit 1
    fi
done
echo ""

# Créer la base de données si elle n'existe pas
echo "🗄️  Création de la base de données ClientConnection..."
docker exec afdb_sqlserver /opt/mssql-tools/bin/sqlcmd \
    -S localhost -U sa -P 'aDb#Cc@Pwd!dev' \
    -Q 'IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'"'"'ClientConnection'"'"') CREATE DATABASE ClientConnection' \
    > /dev/null 2>&1 || true
echo ""

# Build et démarrer l'API
echo "🏗️  Build de l'image Docker de l'API..."
cd Dependencies
docker-compose -f docker-compose.full.yml build api
echo ""

echo "🚀 Démarrage de l'API..."
docker-compose -f docker-compose.full.yml up -d api
cd ..
echo ""

# Attendre que l'API soit prête
echo "⏳ Attente que l'API soit prête (cela peut prendre 30-60 secondes)..."
for i in {1..30}; do
    if curl -f -s http://localhost:5000/health > /dev/null 2>&1; then
        echo -e "${GREEN}✅ L'API est prête!${NC}"
        break
    fi
    echo -n "."
    sleep 2
    if [ $i -eq 30 ]; then
        echo -e "${YELLOW}⚠️  L'API n'a pas répondu à temps${NC}"
        echo "Mais elle démarre peut-être encore. Vérifie les logs avec:"
        echo "  docker logs -f afdb_api"
    fi
done
echo ""

# Appliquer les migrations
echo "📊 Application des migrations Entity Framework..."
if command -v dotnet &> /dev/null; then
    cd src/Afdb.ClientConnection.Infrastructure
    dotnet ef database update \
        --startup-project ../Afdb.ClientConnection.Api \
        --connection "Server=localhost,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;" \
        > /dev/null 2>&1 || {
            echo -e "${YELLOW}⚠️  Impossible d'appliquer les migrations depuis l'hôte${NC}"
            echo "Tu peux les appliquer manuellement depuis le container:"
            echo "  docker exec -it afdb_api dotnet ef database update"
        }
    cd ../..
else
    echo -e "${YELLOW}⚠️  .NET SDK non installé sur l'hôte${NC}"
    echo "Les migrations seront appliquées au premier démarrage de l'API"
fi
echo ""

# Afficher le résumé
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo -e "${GREEN}✅ Environnement local démarré avec succès!${NC}"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""
echo "📍 Services disponibles:"
echo ""
echo "  🌐 API Swagger:     http://localhost:5000/swagger"
echo "  ❤️  Health Check:   http://localhost:5000/health"
echo "  🔌 API Base URL:    http://localhost:5000"
echo "  🗄️  SQL Server:     localhost:5001"
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""
echo "📝 Commandes utiles:"
echo ""
echo "  Voir les logs:           docker logs -f afdb_api"
echo "  Arrêter:                 docker-compose -f Dependencies/docker-compose.full.yml down"
echo "  Redémarrer l'API:        docker restart afdb_api"
echo "  Exec dans l'API:         docker exec -it afdb_api bash"
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""

# Ouvrir Swagger si possible
if command -v open &> /dev/null; then
    echo "🌐 Ouverture de Swagger dans le navigateur..."
    sleep 2
    open http://localhost:5000/swagger
elif command -v xdg-open &> /dev/null; then
    echo "🌐 Ouverture de Swagger dans le navigateur..."
    sleep 2
    xdg-open http://localhost:5000/swagger
fi

echo "🎉 Prêt à développer!"
