FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src





COPY ["Kullanici.API.csproj", "."]

RUN dotnet restore "./Kullanici.API.csproj"

COPY . .

WORKDIR "/src/."

RUN dotnet build "Kullanici.API.csproj" -c Release -o /app/build





FROM build AS publish

RUN dotnet publish "Kullanici.API.csproj" -c Release -o /app/publish /p:UseAppHost=false





FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Kullanici.API.dll"]