FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проекты
COPY ["ProjectManagement.Api/ProjectManagement.Api.csproj", "ProjectManagement.Api/"]
COPY ["ProjectManagement.Infrastructure/ProjectManagement.Infrastructure.csproj", "ProjectManagement.Infrastructure/"]
COPY ["ProjectManagement.Domain/ProjectManagement.Domain.csproj", "ProjectManagement.Domain/"]
COPY ["ProjectManagement.Service/ProjectManagement.Service.csproj", "ProjectManagement.Service/"]

# Восстанавливаем зависимости
RUN dotnet restore "ProjectManagement.Api/ProjectManagement.Api.csproj"
COPY . .
WORKDIR "/src/ProjectManagement.Api"

# Строим приложение
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Применяем миграции (если база данных доступна)
RUN dotnet ef database update --project ProjectManagement.Api.csproj || echo "Database update skipped"

# Переход на runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Запуск API
CMD ["dotnet", "ProjectManagement.Api.dll"]
