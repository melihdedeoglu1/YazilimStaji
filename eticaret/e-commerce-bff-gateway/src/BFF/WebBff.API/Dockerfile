
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebBff.API.csproj", "."]
RUN dotnet restore "./WebBff.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebBff.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "WebBff.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebBff.API.dll"]