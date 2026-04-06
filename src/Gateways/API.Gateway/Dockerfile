FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore src/Gateways/API.Gateway/API.Gateway.csproj
RUN dotnet publish src/Gateways/API.Gateway/API.Gateway.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "API.Gateway.dll"]
