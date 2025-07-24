FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ApiGateway.csproj", "."]

RUN dotnet restore "./ApiGateway.csproj"

COPY . .

WORKDIR "/src/."

RUN dotnet build "ApiGateway.csproj" -c Release -o /app/build





FROM build AS publish

RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false





FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ApiGateway.dll"]