# Ingest

High-throughput event ingestion from SDK clients with message queue publishing.

## Responsibilities

- Receive event batches via REST API
- Publish events to RabbitMQ for downstream processing

## API

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/ingest/v1/organizations/{orgId}/projects/{projectId}/events/batch` | Ingest event batch |

## Dependencies

- **RabbitMQ** — message queue (MassTransit)

## Configuration

| Parameter | Description |
|-----------|-------------|
| `RabbitMQ:Host` | RabbitMQ host |
| `RabbitMQ:Username` / `RabbitMQ:Password` | Credentials |

## Port

- **8083** — HTTP/1.1 + HTTP/2
