FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проектов
COPY ["ProjectManagement.Api/ProjectManagement.Api.csproj", "ProjectManagement.Api/"]
COPY ["ProjectManagement.Infrastructure/ProjectManagement.Infrastructure.csproj", "ProjectManagement.Infrastructure/"]
COPY ["ProjectManagement.Domain/ProjectManagement.Domain.csproj", "ProjectManagement.Domain/"]
COPY ["ProjectManagement.Service/ProjectManagement.Service.csproj", "ProjectManagement.Service/"]

# Восстанавливаем зависимости
RUN dotnet restore "ProjectManagement.Api/ProjectManagement.Api.csproj"
COPY . .
WORKDIR "/src/ProjectManagement.Api"

# Компиляция
RUN dotnet build "ProjectManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Автоматический запуск миграций перед стартом API
CMD ["sh", "-c", "dotnet ef database update --project ../ProjectManagement.Infrastructure/ProjectManagement.Infrastructure.csproj && dotnet ProjectManagement.Api.dll"]
