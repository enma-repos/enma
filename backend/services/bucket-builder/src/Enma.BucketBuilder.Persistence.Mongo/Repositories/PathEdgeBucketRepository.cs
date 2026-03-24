using Enma.BucketBuilder.Application.Contracts.Persistence;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Persistence.Mongo.Connection;
using Enma.BucketBuilder.Persistence.Mongo.Documents;
using FluentResults;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Repositories;

internal sealed class PathEdgeBucketRepository : IPathEdgeBucketRepository
{
    private readonly IMongoCollection<PathEdgeBucketDocument> _collection;

    public PathEdgeBucketRepository(IMongoDbContext context)
    {
        _collection = context.PathEdgeBuckets;
    }

    public async Task<Result> UpsertBatchAsync(IReadOnlyCollection<PathEdgeBucket> buckets, CancellationToken ct = default)
    {
        if (buckets.Count == 0) return Result.Ok();

        var models = new List<WriteModel<PathEdgeBucketDocument>>(buckets.Count);

        foreach (var bucket in buckets)
        {
            var filter = Builders<PathEdgeBucketDocument>.Filter.And(
                Builders<PathEdgeBucketDocument>.Filter.Eq(d => d.OrganizationId, bucket.Key.OrganizationId),
                Builders<PathEdgeBucketDocument>.Filter.Eq(d => d.ProjectId, bucket.Key.ProjectId),
                Builders<PathEdgeBucketDocument>.Filter.Eq(d => d.ProcessDefinitionId, bucket.Key.ProcessDefinitionId),
                Builders<PathEdgeBucketDocument>.Filter.Eq(d => d.FromEvent, bucket.Key.FromEvent.Value),
                Builders<PathEdgeBucketDocument>.Filter.Eq(d => d.ToEvent, bucket.Key.ToEvent.Value),
                Builders<PathEdgeBucketDocument>.Filter.Eq(d => d.EntryEventName, bucket.EntryEventName.Value),
                Builders<PathEdgeBucketDocument>.Filter.Eq(d => d.BucketStartUtc, bucket.Key.BucketStartUtc.Value));

            var doc = new PathEdgeBucketDocument
            {
                OrganizationId = bucket.Key.OrganizationId,
                ProjectId = bucket.Key.ProjectId,
                ProcessDefinitionId = bucket.Key.ProcessDefinitionId,
                FromEvent = bucket.Key.FromEvent.Value,
                ToEvent = bucket.Key.ToEvent.Value,
                EntryEventName = bucket.EntryEventName.Value,
                BucketStartUtc = bucket.Key.BucketStartUtc.Value,
                BucketEndUtc = bucket.BucketEndUtc.Value,
                TransitionsCount = bucket.TransitionsCount,
                UniqueChains = bucket.UniqueChains,
                UniqueUsers = bucket.UniqueUsers,
                UniqueAnonymous = bucket.UniqueAnonymous
            };

            models.Add(new ReplaceOneModel<PathEdgeBucketDocument>(filter, doc) { IsUpsert = true });
        }

        await _collection.BulkWriteAsync(models, cancellationToken: ct);
        return Result.Ok();
    }
}
