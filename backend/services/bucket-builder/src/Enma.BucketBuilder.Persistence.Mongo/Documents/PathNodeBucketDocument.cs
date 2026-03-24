using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Enma.BucketBuilder.Persistence.Mongo.Documents;

internal sealed class PathNodeBucketDocument
{
    [BsonId]
    [BsonIgnoreIfDefault]
    public ObjectId Id { get; set; }

    [BsonElement("org_id")]
    public Guid OrganizationId { get; set; }

    [BsonElement("project_id")]
    public Guid ProjectId { get; set; }

    [BsonElement("process_def_id")]
    public Guid ProcessDefinitionId { get; set; }

    [BsonElement("event_name")]
    public string EventName { get; set; } = string.Empty;

    [BsonElement("entry_event_name")]
    public string EntryEventName { get; set; } = string.Empty;

    [BsonElement("bucket_start_utc")]
    public DateTime BucketStartUtc { get; set; }

    [BsonElement("bucket_end_utc")]
    public DateTime BucketEndUtc { get; set; }

    [BsonElement("visits_count")]
    public long VisitsCount { get; set; }

    [BsonElement("entries_count")]
    public long EntriesCount { get; set; }

    [BsonElement("exits_count")]
    public long ExitsCount { get; set; }

    [BsonElement("unique_chains")]
    public long UniqueChains { get; set; }
}
