# Deployment

## Docker

Each service has a `Dockerfile` in its `src/<ServiceName>.Api/` (or `src/<ServiceName>.Worker/`) directory.



## Configuration

Production settings are provided via environment variables or `appsettings.json` files for each service. Available configuration sections:

- Connection strings (PostgreSQL, MongoDB, ClickHouse, Redis, RabbitMQ)
- JWT parameters (key, issuer, audience)
- CORS origins
- Rate limiting

See individual [service docs](services/) for details.
