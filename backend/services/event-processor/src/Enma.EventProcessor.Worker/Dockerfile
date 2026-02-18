FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/event-processor/Enma.EventProcessor/Enma.EventProcessor.csproj", "services/event-processor/Enma.EventProcessor/"]
RUN dotnet restore "services/event-processor/Enma.EventProcessor/Enma.EventProcessor.csproj"
COPY . .
WORKDIR "/src/services/event-processor/Enma.EventProcessor"
RUN dotnet build "Enma.EventProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Enma.EventProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enma.EventProcessor.dll"]
