FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore src/Web/Blazor.Client/Blazor.Client.csproj
RUN dotnet publish src/Web/Blazor.Client/Blazor.Client.csproj -c Release -o /app

FROM nginx:alpine AS runtime
COPY --from=build /app/wwwroot /usr/share/nginx/html
