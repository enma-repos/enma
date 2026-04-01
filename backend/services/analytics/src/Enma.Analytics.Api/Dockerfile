FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["backend/shared/src/Enma.Common/Enma.Common.csproj", "backend/shared/src/Enma.Common/"]
COPY ["backend/shared/src/Enma.Api.Shared/Enma.Api.Shared.csproj", "backend/shared/src/Enma.Api.Shared/"]
COPY ["backend/services/analytics/src/Enma.Analytics.Application/Enma.Analytics.Application.csproj", "backend/services/analytics/src/Enma.Analytics.Application/"]
COPY ["backend/services/analytics/src/Enma.Analytics.Persistence.Mongo/Enma.Analytics.Persistence.Mongo.csproj", "backend/services/analytics/src/Enma.Analytics.Persistence.Mongo/"]
COPY ["backend/services/analytics/src/Enma.Analytics.Persistence.ClickHouse/Enma.Analytics.Persistence.ClickHouse.csproj", "backend/services/analytics/src/Enma.Analytics.Persistence.ClickHouse/"]
COPY ["backend/services/analytics/src/Enma.Analytics.Api/Enma.Analytics.Api.csproj", "backend/services/analytics/src/Enma.Analytics.Api/"]

RUN dotnet restore "backend/services/analytics/src/Enma.Analytics.Api/Enma.Analytics.Api.csproj"

COPY . .
WORKDIR "/src/backend/services/analytics/src/Enma.Analytics.Api"
RUN dotnet build "Enma.Analytics.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Enma.Analytics.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enma.Analytics.Api.dll"]
