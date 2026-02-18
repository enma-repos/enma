FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/bucket-builder/Enma.BucketBuilder/Enma.BucketBuilder.csproj", "services/bucket-builder/Enma.BucketBuilder/"]
RUN dotnet restore "services/bucket-builder/Enma.BucketBuilder/Enma.BucketBuilder.csproj"
COPY . .
WORKDIR "/src/services/bucket-builder/Enma.BucketBuilder"
RUN dotnet build "Enma.BucketBuilder.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Enma.BucketBuilder.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enma.BucketBuilder.dll"]
