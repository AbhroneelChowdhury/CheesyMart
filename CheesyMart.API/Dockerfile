FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CheesyMart.API/CheesyMart.API.csproj", "CheesyMart.API/"]
COPY ["CheesyMart.Core/CheesyMart.Core.csproj", "CheesyMart.Core/"]
COPY ["CheesyMart.Data/CheesyMart.Data.csproj", "CheesyMart.Data/"]
COPY ["CheesyMart.Infrastructure/CheesyMart.Infrastructure.csproj", "CheesyMart.Infrastructure/"]
RUN dotnet restore "CheesyMart.API/CheesyMart.API.csproj"
COPY . .
WORKDIR "/src/CheesyMart.API"
RUN dotnet build "CheesyMart.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CheesyMart.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CheesyMart.API.dll"]
