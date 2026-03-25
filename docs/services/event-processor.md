# Event Processor

Background worker that consumes events from the message queue and writes them to the OLAP store.

## Responsibilities

- Consume events from RabbitMQ
- Transform and process events
- Write to ClickHouse

## Dependencies

- **RabbitMQ** — event source (MassTransit)
- **ClickHouse** — target OLAP store

## Configuration

| Parameter | Description |
|-----------|-------------|
| `RabbitMQ:Host` | RabbitMQ host |
| `ClickHouse:ConnectionString` | ClickHouse connection string |

## Service Type

Background worker (no REST API).
