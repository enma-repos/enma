using Enma.BucketBuilder.Application.Contracts.Persistence;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Persistence.Mongo.Connection;
using Enma.BucketBuilder.Persistence.Mongo.Documents;
using FluentResults;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Repositories;

internal sealed class PathNodeBucketRepository : IPathNodeBucketRepository
{
    private readonly IMongoCollection<PathNodeBucketDocument> _collection;

    public PathNodeBucketRepository(IMongoDbContext context)
    {
        _collection = context.PathNodeBuckets;
    }

    public async Task<Result> UpsertBatchAsync(IReadOnlyCollection<PathNodeBucket> buckets, CancellationToken ct = default)
    {
        if (buckets.Count == 0) return Result.Ok();

        var models = new List<WriteModel<PathNodeBucketDocument>>(buckets.Count);

        foreach (var bucket in buckets)
        {
            var filter = Builders<PathNodeBucketDocument>.Filter.And(
                Builders<PathNodeBucketDocument>.Filter.Eq(d => d.OrganizationId, bucket.Key.OrganizationId),
                Builders<PathNodeBucketDocument>.Filter.Eq(d => d.ProjectId, bucket.Key.ProjectId),
                Builders<PathNodeBucketDocument>.Filter.Eq(d => d.ProcessDefinitionId, bucket.Key.ProcessDefinitionId),
                Builders<PathNodeBucketDocument>.Filter.Eq(d => d.EventName, bucket.Key.EventName.Value),
                Builders<PathNodeBucketDocument>.Filter.Eq(d => d.BucketStartUtc, bucket.Key.BucketStartUtc.Value));

            var doc = new PathNodeBucketDocument
            {
                OrganizationId = bucket.Key.OrganizationId,
                ProjectId = bucket.Key.ProjectId,
                ProcessDefinitionId = bucket.Key.ProcessDefinitionId,
                EventName = bucket.Key.EventName.Value,
                BucketStartUtc = bucket.Key.BucketStartUtc.Value,
                BucketEndUtc = bucket.BucketEndUtc.Value,
                VisitsCount = bucket.VisitsCount,
                EntriesCount = bucket.EntriesCount,
                ExitsCount = bucket.ExitsCount,
                UniqueChains = bucket.UniqueChains
            };

            models.Add(new ReplaceOneModel<PathNodeBucketDocument>(filter, doc) { IsUpsert = true });
        }

        await _collection.BulkWriteAsync(models, cancellationToken: ct);
        return Result.Ok();
    }
}
