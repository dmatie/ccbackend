# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

# Cet index est utilisé lors de l’exécution à partir de VS en mode rapide (par défaut pour la configuration de débogage)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Cette phase est utilisée pour générer le projet de service
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Afdb.ClientConnection.Api/Afdb.ClientConnection.Api.csproj", "src/Afdb.ClientConnection.Api/"]
COPY ["src/Afdb.ClientConnection.Application/Afdb.ClientConnection.Application.csproj", "src/Afdb.ClientConnection.Application/"]
COPY ["src/Afdb.ClientConnection.Domain/Afdb.ClientConnection.Domain.csproj", "src/Afdb.ClientConnection.Domain/"]
COPY ["src/Afdb.ClientConnection.Infrastructure/Afdb.ClientConnection.Infrastructure.csproj", "src/Afdb.ClientConnection.Infrastructure/"]
RUN dotnet restore "./src/Afdb.ClientConnection.Api/Afdb.ClientConnection.Api.csproj"
COPY . .
WORKDIR "/src/src/Afdb.ClientConnection.Api"
RUN dotnet build "./Afdb.ClientConnection.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Cette étape permet de publier le projet de service à copier dans la phase finale
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Afdb.ClientConnection.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Cette phase est utilisée en production ou lors de l’exécution à partir de VS en mode normal (par défaut quand la configuration de débogage n’est pas utilisée)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Afdb.ClientConnection.Api.dll"]