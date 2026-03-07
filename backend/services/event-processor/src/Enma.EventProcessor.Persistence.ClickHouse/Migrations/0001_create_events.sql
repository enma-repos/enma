CREATE TABLE IF NOT EXISTS events
(
    -- Идентичность
    event_id UUID,
    organization_id UUID,
    project_id UUID,
    sdk_client_id UUID,

    -- Семантика события
    event_name LowCardinality(String),
    occurred_at DateTime64(3, 'UTC'),
    ingested_at DateTime64(3, 'UTC'),

    -- Actor
    actor_user_id Nullable(String),
    actor_anonymous_id Nullable(String),

    -- ProcessKeys из EventMessageDto
    process_keys Nested
                 (
                 process_definition_id UUID,
                 process_id String
                 ),

    -- Теги и payload
    tags Map(String, String),
    payload_json String CODEC(ZSTD(3)),

    -- Технические метаданные ingestion
    rabbit_message_id Nullable(UUID),
    rabbit_routing_key LowCardinality(String) DEFAULT '',
    rabbit_received_at DateTime64(3, 'UTC') DEFAULT now64(3),
    schema_version UInt16 DEFAULT 1
)

ENGINE = MergeTree
PARTITION BY toYYYYMM(occurred_at)
ORDER BY (organization_id, project_id, event_name, occurred_at, event_id);
