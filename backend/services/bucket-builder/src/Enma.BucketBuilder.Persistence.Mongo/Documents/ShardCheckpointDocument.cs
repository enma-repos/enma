using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Enma.BucketBuilder.Persistence.Mongo.Documents;

internal sealed class ShardCheckpointDocument
{
    [BsonId]
    [BsonIgnoreIfDefault]
    public ObjectId Id { get; set; }

    [BsonElement("pipeline")]
    public string Pipeline { get; set; } = string.Empty;

    [BsonElement("shard_index")]
    public int ShardIndex { get; set; }

    [BsonElement("shard_count")]
    public int ShardCount { get; set; }

    [BsonElement("last_completed_bucket_end_utc")]
    [BsonIgnoreIfNull]
    public DateTime? LastCompletedBucketEndUtc { get; set; }

    [BsonElement("lease_owner")]
    [BsonIgnoreIfNull]
    public string? LeaseOwner { get; set; }

    [BsonElement("lease_until_utc")]
    [BsonIgnoreIfNull]
    public DateTime? LeaseUntilUtc { get; set; }

    [BsonElement("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; set; }
}
