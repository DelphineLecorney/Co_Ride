FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Gateways/API.Gateway/API.Gateway.csproj", "src/Gateways/API.Gateway/"]
COPY ["src/BuildingBlocks/Shared.Contracts/Shared.Contracts.csproj", "src/BuildingBlocks/Shared.Contracts/"]

RUN dotnet restore "src/Gateways/API.Gateway/API.Gateway.csproj"

COPY . .

WORKDIR "/src/src/Gateways/API.Gateway"
RUN dotnet publish "API.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.Gateway.dll"]