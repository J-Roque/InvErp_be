# Dockerfile para Security.API - InvERP
# Basado en .NET 8.0 para desarrollo local

# Etapa base - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de build - SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos de proyecto en orden de dependencias
COPY ["Services/Security/Security.API/Security.API.csproj", "Services/Security/Security.API/"]
COPY ["Services/Security/Security.Application/Security.Application.csproj", "Services/Security/Security.Application/"]
COPY ["Services/Security/Security.Domain/Security.Domain.csproj", "Services/Security/Security.Domain/"]
COPY ["Services/Security/Security.Infrastructure/Security.Infrastructure.csproj", "Services/Security/Security.Infrastructure/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["Utilities/Common/Common.csproj", "Utilities/Common/"]

# Restaurar dependencias
RUN dotnet restore "Services/Security/Security.API/Security.API.csproj"

# Copiar todo el código fuente
COPY . .

# Build del proyecto
WORKDIR "/src/Services/Security/Security.API"
RUN dotnet build "Security.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Security.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final - Imagen de producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Security.API.dll"]
