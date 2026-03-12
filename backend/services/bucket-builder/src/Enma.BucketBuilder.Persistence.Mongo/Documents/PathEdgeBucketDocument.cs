using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Enma.BucketBuilder.Persistence.Mongo.Documents;

internal sealed class PathEdgeBucketDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("org_id")]
    public Guid OrganizationId { get; set; }

    [BsonElement("project_id")]
    public Guid ProjectId { get; set; }

    [BsonElement("process_def_id")]
    public Guid ProcessDefinitionId { get; set; }

    [BsonElement("from_event")]
    public string FromEvent { get; set; } = string.Empty;

    [BsonElement("to_event")]
    public string ToEvent { get; set; } = string.Empty;

    [BsonElement("bucket_start_utc")]
    public DateTime BucketStartUtc { get; set; }

    [BsonElement("bucket_end_utc")]
    public DateTime BucketEndUtc { get; set; }

    [BsonElement("transitions_count")]
    public long TransitionsCount { get; set; }

    [BsonElement("unique_chains")]
    public long UniqueChains { get; set; }

    [BsonElement("unique_users")]
    public long UniqueUsers { get; set; }

    [BsonElement("unique_anonymous")]
    public long UniqueAnonymous { get; set; }
}
