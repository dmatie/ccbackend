@echo off
REM Script de démarrage rapide pour Docker Desktop (Windows)
REM Usage: start-local.bat

echo.
echo ================================
echo AfDB Client Connection - Local
echo ================================
echo.

REM Vérifier que Docker est démarré
docker info >nul 2>&1
if errorlevel 1 (
    echo [ERREUR] Docker n'est pas demarre.
    echo Demarre Docker Desktop et reessaye.
    pause
    exit /b 1
)

echo [OK] Docker est demarre
echo.

REM Nettoyer les anciens containers
echo Nettoyage des anciens containers...
cd Dependencies
docker-compose -f docker-compose.full.yml down >nul 2>&1
cd ..
echo.

REM Démarrer SQL Server
echo Demarrage de SQL Server...
cd Dependencies
docker-compose -f docker-compose.full.yml up -d sqlserver
cd ..
echo.

REM Attendre que SQL Server soit prêt
echo Attente que SQL Server soit pret (30-60 secondes)...
timeout /t 10 /nobreak >nul
:wait_sql
docker exec afdb_sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "aDb#Cc@Pwd!dev" -Q "SELECT 1" >nul 2>&1
if errorlevel 1 (
    echo Attente...
    timeout /t 5 /nobreak >nul
    goto wait_sql
)
echo [OK] SQL Server est pret!
echo.

REM Créer la base de données
echo Creation de la base de donnees ClientConnection...
docker exec afdb_sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "aDb#Cc@Pwd!dev" -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ClientConnection') CREATE DATABASE ClientConnection" >nul 2>&1
echo.

REM Build de l'API
echo Build de l'image Docker de l'API...
cd Dependencies
docker-compose -f docker-compose.full.yml build api
echo.

echo Demarrage de l'API...
docker-compose -f docker-compose.full.yml up -d api
cd ..
echo.

REM Attendre que l'API soit prête
echo Attente que l'API soit prete (30-60 secondes)...
timeout /t 15 /nobreak >nul
:wait_api
curl -f -s http://localhost:5000/health >nul 2>&1
if errorlevel 1 (
    echo Attente...
    timeout /t 5 /nobreak >nul
    goto wait_api
)
echo [OK] L'API est prete!
echo.

REM Appliquer les migrations
echo Application des migrations Entity Framework...
where dotnet >nul 2>&1
if not errorlevel 1 (
    cd src\Afdb.ClientConnection.Infrastructure
    dotnet ef database update --startup-project ..\Afdb.ClientConnection.Api --connection "Server=localhost,5001;Database=ClientConnection;User Id=sa;Password=aDb#Cc@Pwd!dev;TrustServerCertificate=True;" >nul 2>&1
    cd ..\..
) else (
    echo [INFO] .NET SDK non installe, migrations seront appliquees au demarrage
)
echo.

REM Afficher le résumé
echo ================================================
echo [OK] Environnement local demarre avec succes!
echo ================================================
echo.
echo Services disponibles:
echo.
echo   API Swagger:     http://localhost:5000/swagger
echo   Health Check:    http://localhost:5000/health
echo   API Base URL:    http://localhost:5000
echo   SQL Server:      localhost:5001
echo.
echo ================================================
echo.
echo Commandes utiles:
echo.
echo   Voir les logs:           docker logs -f afdb_api
echo   Arreter:                 docker-compose -f Dependencies\docker-compose.full.yml down
echo   Redemarrer l'API:        docker restart afdb_api
echo   Exec dans l'API:         docker exec -it afdb_api bash
echo.
echo ================================================
echo.

REM Ouvrir Swagger
start http://localhost:5000/swagger

echo Pret a developper!
echo.
pause
