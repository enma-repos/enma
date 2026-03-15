using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Enma.BucketBuilder.Persistence.Mongo.Documents;

internal sealed class ChainCursorDocument
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

    [BsonElement("process_id")]
    public string ProcessId { get; set; } = string.Empty;

    [BsonElement("last_event_id")]
    public Guid LastEventId { get; set; }

    [BsonElement("last_event_name")]
    public string LastEventName { get; set; } = string.Empty;

    [BsonElement("last_occurred_at_utc")]
    public DateTime LastOccurredAtUtc { get; set; }

    [BsonElement("last_actor_user_id")]
    [BsonIgnoreIfNull]
    public string? LastActorUserId { get; set; }

    [BsonElement("last_actor_anonymous_id")]
    [BsonIgnoreIfNull]
    public string? LastActorAnonymousId { get; set; }

    [BsonElement("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; set; }
}
