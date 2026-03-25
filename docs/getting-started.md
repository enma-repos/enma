# Getting Started

## Prerequisites

- Docker and Docker Compose

## Setup

1. Copy the example environment file and adjust values if needed:

```bash
cd deploy/local
cp .env.example .env
```

2. Start infrastructure (PostgreSQL, MongoDB, ClickHouse, Redis, RabbitMQ):

```bash
docker compose -f compose.infra.yml up -d
```

3. Start all services:

```bash
docker compose -f compose.services.yml up -d
```

## Seed Data

```bash
cd backend/tools/seed
npm install
node seed.mjs --org-id <guid> --project-id <guid> --scenario ecommerce
```

See `backend/tools/seed/seed.mjs` for all available options.

## Environment Variables

All configuration is managed through the `.env` file in `deploy/local/`. See `.env.example` for the full list of available variables with default values.

Key variable groups:

| Group | Examples |
|-------|---------|
| **Postgres** | `POSTGRES_HOST`, `POSTGRES_PORT`, `POSTGRES_DB`, `POSTGRES_USER`, `POSTGRES_PASSWORD` |
| **ClickHouse** | `CLICKHOUSE_HOST`, `CLICKHOUSE_HTTP_PORT`, `CLICKHOUSE_NATIVE_PORT`, `CLICKHOUSE_CONNECTION` |
| **MongoDB** | `MONGO_HOST`, `MONGO_PORT`, `MONGO_CONNECTION_STRING` |
| **Redis** | `REDIS_HOST`, `REDIS_PORT`, `REDIS_CONNECTION_STRING` |
| **RabbitMQ** | `RABBITMQ_HOST`, `RABBITMQ_PORT`, `RABBITMQ_URI` |
| **Auth / JWT** | `AUTH_JWT_SECRET`, `JWT_EXPIRES_MINUTES`, `AUTH_REFRESH_TOKEN_LIFETIME_DAYS` |
| **Google OAuth** | `GOOGLE_CLIENT_ID`, `GOOGLE_CLIENT_SECRET`, `AUTH_GOOGLE_REDIRECT_URL` |
| **Event Processor** | `EP_RABBITMQ_PREFETCH_COUNT`, `EP_RABBITMQ_BATCH_MESSAGE_LIMIT` |
| **Bucket Builder** | `BB_POLL_INTERVAL_SECONDS`, `BB_SHARD_COUNT`, `BB_SAFETY_LAG_SECONDS` |
