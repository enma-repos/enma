FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/ingest/Enma.Ingest/Enma.Ingest.csproj", "services/ingest/Enma.Ingest/"]
RUN dotnet restore "services/ingest/Enma.Ingest/Enma.Ingest.csproj"
COPY . .
WORKDIR "/src/services/ingest/Enma.Ingest"
RUN dotnet build "Enma.Ingest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Enma.Ingest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enma.Ingest.dll"]
