using Enma.BucketBuilder.Persistence.Mongo.Documents;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Configurations;

internal static class PathNodeBucketsCollectionSetup
{
    internal static void EnsureIndexes(IMongoCollection<PathNodeBucketDocument> collection)
    {
        var index = new CreateIndexModel<PathNodeBucketDocument>(
            Builders<PathNodeBucketDocument>.IndexKeys
                .Ascending(d => d.OrganizationId)
                .Ascending(d => d.ProjectId)
                .Ascending(d => d.ProcessDefinitionId)
                .Ascending(d => d.EventName)
                .Ascending(d => d.BucketStartUtc),
            new CreateIndexOptions { Unique = true, Name = "uq_node_bucket_key" });

        collection.Indexes.CreateOne(index);
    }
}
