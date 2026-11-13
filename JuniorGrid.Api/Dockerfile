# Etapa 1: Compilación / Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["JuniorGrid.Api.csproj", "./"]
RUN dotnet restore "JuniorGrid.Api.csproj"

# Copiar todo el código fuente restante y compilar
COPY . .
RUN dotnet build "JuniorGrid.Api.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "JuniorGrid.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Imagen final para ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JuniorGrid.Api.dll"]