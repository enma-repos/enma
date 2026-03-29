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
COPY ["backend/services/auth/src/Enma.Auth.Application/Enma.Auth.Application.csproj", "backend/services/auth/src/Enma.Auth.Application/"]
COPY ["backend/services/auth/src/Enma.Auth.Infrastructure.Security/Enma.Auth.Infrastructure.Security.csproj", "backend/services/auth/src/Enma.Auth.Infrastructure.Security/"]
COPY ["backend/services/auth/src/Enma.Auth.Infrastructure.Caching/Enma.Auth.Infrastructure.Caching.csproj", "backend/services/auth/src/Enma.Auth.Infrastructure.Caching/"]
COPY ["backend/services/auth/src/Enma.Auth.Infrastructure.ExternalAuth.Google/Enma.Auth.Infrastructure.ExternalAuth.Google.csproj", "backend/services/auth/src/Enma.Auth.Infrastructure.ExternalAuth.Google/"]
COPY ["backend/services/auth/src/Enma.Auth.Infrastructure.Grpc.Admin/Enma.Auth.Infrastructure.Grpc.Admin.csproj", "backend/services/auth/src/Enma.Auth.Infrastructure.Grpc.Admin/"]
COPY ["backend/services/auth/src/Enma.Auth.Persistence.Postgres/Enma.Auth.Persistence.Postgres.csproj", "backend/services/auth/src/Enma.Auth.Persistence.Postgres/"]
COPY ["backend/services/auth/src/Enma.Auth.Api/Enma.Auth.Api.csproj", "backend/services/auth/src/Enma.Auth.Api/"]

RUN dotnet restore "backend/services/auth/src/Enma.Auth.Api/Enma.Auth.Api.csproj"

COPY . .
WORKDIR "/src/backend/services/auth/src/Enma.Auth.Api"
RUN dotnet build "Enma.Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Enma.Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enma.Auth.Api.dll"]
