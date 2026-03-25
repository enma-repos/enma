# Bucket Builder

Scheduled worker that builds analytical buckets from raw data.

## Responsibilities

- Periodically read raw data from MongoDB
- Build aggregates and buckets
- Write results to ClickHouse
- Shard-based parallel processing

## Dependencies

- **MongoDB** — raw data source
- **ClickHouse** — target aggregate store
- **Quartz** — job scheduler

## Configuration

| Parameter | Description |
|-----------|-------------|
| `MongoDB:ConnectionString` | Connection string |
| `ClickHouse:ConnectionString` | Connection string |
| `BucketBuilder:ShardCount` | Number of shards (default: 1) |
| `BucketBuilder:PollIntervalSeconds` | Poll interval (default: 10) |

## Service Type

Background worker (Quartz scheduler, no REST API).
