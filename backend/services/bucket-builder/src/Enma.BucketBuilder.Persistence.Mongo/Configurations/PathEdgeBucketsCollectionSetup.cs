using Enma.BucketBuilder.Persistence.Mongo.Documents;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Configurations;

internal static class PathEdgeBucketsCollectionSetup
{
    internal static void EnsureIndexes(IMongoCollection<PathEdgeBucketDocument> collection)
    {
        var index = new CreateIndexModel<PathEdgeBucketDocument>(
            Builders<PathEdgeBucketDocument>.IndexKeys
                .Ascending(d => d.OrganizationId)
                .Ascending(d => d.ProjectId)
                .Ascending(d => d.ProcessDefinitionId)
                .Ascending(d => d.FromEvent)
                .Ascending(d => d.ToEvent)
                .Ascending(d => d.EntryEventName)
                .Ascending(d => d.BucketStartUtc),
            new CreateIndexOptions { Unique = true, Name = "uq_edge_bucket_key_v2" });

        collection.Indexes.CreateOne(index);
    }
}
