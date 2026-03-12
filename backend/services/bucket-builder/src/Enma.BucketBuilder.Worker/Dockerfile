FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files for restore
COPY ["backend/shared/src/Enma.Common/Enma.Common.csproj", "backend/shared/src/Enma.Common/"]
COPY ["backend/services/bucket-builder/src/Enma.BucketBuilder.Application/Enma.BucketBuilder.Application.csproj", "backend/services/bucket-builder/src/Enma.BucketBuilder.Application/"]
COPY ["backend/services/bucket-builder/src/Enma.BucketBuilder.JobsOrchestration/Enma.BucketBuilder.JobsOrchestration.csproj", "backend/services/bucket-builder/src/Enma.BucketBuilder.JobsOrchestration/"]
COPY ["backend/services/bucket-builder/src/Enma.BucketBuilder.Persistence.ClickHouse/Enma.BucketBuilder.Persistence.ClickHouse.csproj", "backend/services/bucket-builder/src/Enma.BucketBuilder.Persistence.ClickHouse/"]
COPY ["backend/services/bucket-builder/src/Enma.BucketBuilder.Persistence.Mongo/Enma.BucketBuilder.Persistence.Mongo.csproj", "backend/services/bucket-builder/src/Enma.BucketBuilder.Persistence.Mongo/"]
COPY ["backend/services/bucket-builder/src/Enma.BucketBuilder.Worker/Enma.BucketBuilder.Worker.csproj", "backend/services/bucket-builder/src/Enma.BucketBuilder.Worker/"]

RUN dotnet restore "backend/services/bucket-builder/src/Enma.BucketBuilder.Worker/Enma.BucketBuilder.Worker.csproj"

COPY . .
WORKDIR "/src/backend/services/bucket-builder/src/Enma.BucketBuilder.Worker"
RUN dotnet build "Enma.BucketBuilder.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Enma.BucketBuilder.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enma.BucketBuilder.Worker.dll"]
