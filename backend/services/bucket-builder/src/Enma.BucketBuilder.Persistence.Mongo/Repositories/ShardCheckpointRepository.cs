using Enma.BucketBuilder.JobsOrchestration.Contracts;
using Enma.BucketBuilder.JobsOrchestration.Models;
using Enma.BucketBuilder.JobsOrchestration.ValueObjects;
using Enma.BucketBuilder.Persistence.Mongo.Connection;
using Enma.BucketBuilder.Persistence.Mongo.Documents;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Repositories;

internal sealed class ShardCheckpointRepository : IShardCheckpointRepository
{
    private readonly IMongoCollection<ShardCheckpointDocument> _collection;

    public ShardCheckpointRepository(IMongoDbContext context)
    {
        _collection = context.ShardCheckpoints;
    }

    public async Task<ShardCheckpoint?> LoadAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        CancellationToken ct = default)
    {
        var filter = BuildShardFilter(pipeline, shard);
        var doc = await _collection.Find(filter).FirstOrDefaultAsync(ct);

        if (doc is null) return null;

        return ShardCheckpoint.Rehydrate(
            doc.Pipeline,
            ShardDescriptor.Rehydrate(doc.ShardIndex, doc.ShardCount),
            doc.LastCompletedBucketEndUtc.HasValue
                ? DateTime.SpecifyKind(doc.LastCompletedBucketEndUtc.Value, DateTimeKind.Utc)
                : null,
            doc.LeaseOwner,
            doc.LeaseUntilUtc.HasValue
                ? DateTime.SpecifyKind(doc.LeaseUntilUtc.Value, DateTimeKind.Utc)
                : null,
            DateTime.SpecifyKind(doc.UpdatedAtUtc, DateTimeKind.Utc));
    }

    public async Task SaveAsync(ShardCheckpoint checkpoint, CancellationToken ct = default)
    {
        var filter = BuildShardFilter(checkpoint.Pipeline, checkpoint.Shard);

        var doc = new ShardCheckpointDocument
        {
            Pipeline = checkpoint.Pipeline.Value,
            ShardIndex = checkpoint.Shard.Index,
            ShardCount = checkpoint.Shard.Count,
            LastCompletedBucketEndUtc = checkpoint.LastCompletedBucketEndUtc?.Value,
            LeaseOwner = checkpoint.LeaseOwner?.Value,
            LeaseUntilUtc = checkpoint.LeaseUntilUtc,
            UpdatedAtUtc = checkpoint.UpdatedAtUtc
        };

        await _collection.ReplaceOneAsync(filter, doc, new ReplaceOptions { IsUpsert = true }, ct);
    }

    public async Task SaveAndRenewLeaseAsync(
        ShardCheckpoint checkpoint,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var filter = Builders<ShardCheckpointDocument>.Filter.And(
            BuildShardFilter(checkpoint.Pipeline, checkpoint.Shard),
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.LeaseOwner, ownerId.Value));

        var update = Builders<ShardCheckpointDocument>.Update
            .Set(d => d.LastCompletedBucketEndUtc, checkpoint.LastCompletedBucketEndUtc?.Value)
            .Set(d => d.LeaseUntilUtc, now + leaseTimeout)
            .Set(d => d.UpdatedAtUtc, now);

        await _collection.UpdateOneAsync(filter, update, cancellationToken: ct);
    }

    public async Task<LeaseToken?> TryAcquireLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var expiresAt = now + leaseTimeout;

        var filter = Builders<ShardCheckpointDocument>.Filter.And(
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.Pipeline, pipeline.Value),
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.ShardIndex, shard.Index),
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.ShardCount, shard.Count),
            Builders<ShardCheckpointDocument>.Filter.Or(
                Builders<ShardCheckpointDocument>.Filter.Eq(d => d.LeaseOwner, null),
                Builders<ShardCheckpointDocument>.Filter.Lt(d => d.LeaseUntilUtc, now),
                Builders<ShardCheckpointDocument>.Filter.Eq(d => d.LeaseOwner, ownerId.Value)));

        var update = Builders<ShardCheckpointDocument>.Update
            .Set(d => d.LeaseOwner, ownerId.Value)
            .Set(d => d.LeaseUntilUtc, expiresAt)
            .Set(d => d.UpdatedAtUtc, now)
            .SetOnInsert(d => d.Pipeline, pipeline.Value)
            .SetOnInsert(d => d.ShardIndex, shard.Index)
            .SetOnInsert(d => d.ShardCount, shard.Count);

        var options = new FindOneAndUpdateOptions<ShardCheckpointDocument>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var doc = await _collection.FindOneAndUpdateAsync(filter, update, options, ct);

        if (doc is null) return null;

        return LeaseToken.Rehydrate(
            pipeline.Value,
            shard,
            ownerId.Value,
            now,
            expiresAt);
    }

    public async Task ReleaseLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        CancellationToken ct = default)
    {
        var filter = Builders<ShardCheckpointDocument>.Filter.And(
            BuildShardFilter(pipeline, shard),
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.LeaseOwner, ownerId.Value));

        var update = Builders<ShardCheckpointDocument>.Update
            .Set(d => d.LeaseOwner, (string?)null)
            .Set(d => d.LeaseUntilUtc, (DateTime?)null)
            .Set(d => d.UpdatedAtUtc, DateTime.UtcNow);

        await _collection.UpdateOneAsync(filter, update, cancellationToken: ct);
    }

    public async Task RenewLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var filter = Builders<ShardCheckpointDocument>.Filter.And(
            BuildShardFilter(pipeline, shard),
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.LeaseOwner, ownerId.Value));

        var update = Builders<ShardCheckpointDocument>.Update
            .Set(d => d.LeaseUntilUtc, now + leaseTimeout)
            .Set(d => d.UpdatedAtUtc, now);

        await _collection.UpdateOneAsync(filter, update, cancellationToken: ct);
    }

    private static FilterDefinition<ShardCheckpointDocument> BuildShardFilter(
        PipelineName pipeline,
        ShardDescriptor shard)
    {
        return Builders<ShardCheckpointDocument>.Filter.And(
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.Pipeline, pipeline.Value),
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.ShardIndex, shard.Index),
            Builders<ShardCheckpointDocument>.Filter.Eq(d => d.ShardCount, shard.Count));
    }
}
