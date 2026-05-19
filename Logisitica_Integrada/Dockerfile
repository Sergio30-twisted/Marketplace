# 1. Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copiamos todo el contenido
COPY . .

# Restauramos y publicamos limpiamente
RUN dotnet restore "Logisitica_Integrada.csproj"
RUN dotnet publish "Logisitica_Integrada.csproj" -c Release -o /app/out

# 2. Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Logisitica_Integrada.dll"]