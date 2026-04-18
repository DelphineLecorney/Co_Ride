# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier uniquement les csproj pour optimiser le cache
COPY src/BuildingBlocks/Shared.Messaging/*.csproj src/BuildingBlocks/Shared.Messaging/
COPY src/BuildingBlocks/Shared.Contracts/*.csproj src/BuildingBlocks/Shared.Contracts/

COPY src/Services/Identity.Service/Identity.API/*.csproj src/Services/Identity.Service/Identity.API/
COPY src/Services/Identity.Service/Identity.Application/*.csproj src/Services/Identity.Service/Identity.Application/
COPY src/Services/Identity.Service/Identity.Infrastructure/*.csproj src/Services/Identity.Service/Identity.Infrastructure/
COPY src/Services/Identity.Service/Identity.Domain/*.csproj src/Services/Identity.Service/Identity.Domain/

RUN dotnet restore src/Services/Identity.Service/Identity.API/Identity.API.csproj

# Copier le reste du code (IMPORTANT : éviter le double src/src)
COPY src/ src/

RUN dotnet publish src/Services/Identity.Service/Identity.Service/Identity.API/Identity.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Identity.API.dll"]
