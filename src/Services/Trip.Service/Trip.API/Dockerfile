FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore src/Services/Trip.Service/Trip.API/Trip.API.csproj
RUN dotnet publish src/Services/Trip.Service/Trip.API/Trip.API.csproj -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Trip.API.dll"]
