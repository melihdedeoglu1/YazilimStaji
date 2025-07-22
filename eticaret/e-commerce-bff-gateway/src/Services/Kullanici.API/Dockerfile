# Aþama 1: Build Ortamý
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece .csproj yerine, projenin TÜM dosyalarýný tek seferde kopyala
COPY ["Kullanici.API.csproj", "."]
RUN dotnet restore "./Kullanici.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Kullanici.API.csproj" -c Release -o /app/build

# Aþama 2: Yayýnlama
FROM build AS publish
RUN dotnet publish "Kullanici.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Aþama 3: Son Hali (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kullanici.API.dll"]