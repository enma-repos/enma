using Enma.BucketBuilder.Persistence.Mongo.Documents;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Configurations;

internal static class ShardCheckpointsCollectionSetup
{
    internal static void EnsureIndexes(IMongoCollection<ShardCheckpointDocument> collection)
    {
        var index = new CreateIndexModel<ShardCheckpointDocument>(
            Builders<ShardCheckpointDocument>.IndexKeys
                .Ascending(d => d.Pipeline)
                .Ascending(d => d.ShardIndex)
                .Ascending(d => d.ShardCount),
            new CreateIndexOptions { Unique = true, Name = "uq_shard_checkpoint_key" });

        collection.Indexes.CreateOne(index);
    }
}
