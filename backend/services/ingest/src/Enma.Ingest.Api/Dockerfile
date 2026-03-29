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
COPY ["backend/services/ingest/src/Enma.Ingest.Application/Enma.Ingest.Application.csproj", "backend/services/ingest/src/Enma.Ingest.Application/"]
COPY ["backend/services/ingest/src/Enma.Ingest.Infrastructure.RabbitMq/Enma.Ingest.Infrastructure.RabbitMq.csproj", "backend/services/ingest/src/Enma.Ingest.Infrastructure.RabbitMq/"]
COPY ["backend/services/ingest/src/Enma.Ingest.Api/Enma.Ingest.Api.csproj", "backend/services/ingest/src/Enma.Ingest.Api/"]

RUN dotnet restore "backend/services/ingest/src/Enma.Ingest.Api/Enma.Ingest.Api.csproj"

COPY . .
WORKDIR "/src/backend/services/ingest/src/Enma.Ingest.Api"
RUN dotnet build "Enma.Ingest.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Enma.Ingest.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enma.Ingest.Api.dll"]
