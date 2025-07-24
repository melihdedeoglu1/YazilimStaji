
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["e-commerce-bff-gateway.sln", "./"]
COPY ["src/BFF/WebBff.API/WebBff.API.csproj", "src/BFF/WebBff.API/"]
COPY ["src/Common/Shared.Contracts/Shared.Contracts.csproj", "src/Common/Shared.Contracts/"]
COPY ["src/Gateway/ApiGateway/ApiGateway.csproj", "src/Gateway/ApiGateway/"]
COPY ["src/Services/Kullanici.API/Kullanici.API.csproj", "src/Services/Kullanici.API/"]
COPY ["src/Services/Siparis.API/Siparis.API.csproj", "src/Services/Siparis.API/"]
COPY ["src/Services/Urun.API/Urun.API.csproj", "src/Services/Urun.API/"]
COPY ["src/Workers/Notifikasyon.Worker/Notifikasyon.Worker.csproj", "src/Workers/Notifikasyon.Worker/"]


RUN dotnet restore "e-commerce-bff-gateway.sln"


COPY . .


WORKDIR "/src/src/Services/Siparis.API"
RUN dotnet publish "Siparis.API.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Siparis.API.dll"]