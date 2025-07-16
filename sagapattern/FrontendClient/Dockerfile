# 1. Çalışma zamanı imajı
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# 2. Build imajı
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyasını kopyala ve restore et
COPY *.csproj ./
RUN dotnet restore

# Tüm dosyaları kopyala ve publish et
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# 3. Final imaj
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Uygulama başlat
ENTRYPOINT ["dotnet", "FrontendClient.dll"]
