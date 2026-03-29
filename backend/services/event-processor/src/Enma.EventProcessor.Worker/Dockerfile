FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["backend/shared/src/Enma.Common/Enma.Common.csproj", "backend/shared/src/Enma.Common/"]
COPY ["backend/services/event-processor/src/Enma.EventProcessor.Application/Enma.EventProcessor.Application.csproj", "backend/services/event-processor/src/Enma.EventProcessor.Application/"]
COPY ["backend/services/event-processor/src/Enma.EventProcessor.Infrastructure.RabbitMq/Enma.EventProcessor.Infrastructure.RabbitMq.csproj", "backend/services/event-processor/src/Enma.EventProcessor.Infrastructure.RabbitMq/"]
COPY ["backend/services/event-processor/src/Enma.EventProcessor.Persistence.ClickHouse/Enma.EventProcessor.Persistence.ClickHouse.csproj", "backend/services/event-processor/src/Enma.EventProcessor.Persistence.ClickHouse/"]
COPY ["backend/services/event-processor/src/Enma.EventProcessor.Worker/Enma.EventProcessor.Worker.csproj", "backend/services/event-processor/src/Enma.EventProcessor.Worker/"]

RUN dotnet restore "backend/services/event-processor/src/Enma.EventProcessor.Worker/Enma.EventProcessor.Worker.csproj"

COPY . .
WORKDIR "/src/backend/services/event-processor/src/Enma.EventProcessor.Worker"
RUN dotnet build "Enma.EventProcessor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Enma.EventProcessor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enma.EventProcessor.Worker.dll"]
