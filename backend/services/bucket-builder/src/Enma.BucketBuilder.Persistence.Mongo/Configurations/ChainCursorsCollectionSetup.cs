using Enma.BucketBuilder.Persistence.Mongo.Documents;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Configurations;

internal static class ChainCursorsCollectionSetup
{
    internal static void EnsureIndexes(IMongoCollection<ChainCursorDocument> collection)
    {
        var uniqueIndex = new CreateIndexModel<ChainCursorDocument>(
            Builders<ChainCursorDocument>.IndexKeys
                .Ascending(d => d.OrganizationId)
                .Ascending(d => d.ProjectId)
                .Ascending(d => d.ProcessDefinitionId)
                .Ascending(d => d.ProcessId),
            new CreateIndexOptions { Unique = true, Name = "uq_chain_cursor_key" });

        var ttlIndex = new CreateIndexModel<ChainCursorDocument>(
            Builders<ChainCursorDocument>.IndexKeys.Ascending(d => d.UpdatedAtUtc),
            new CreateIndexOptions { ExpireAfter = TimeSpan.FromHours(24), Name = "ttl_updated_at" });

        collection.Indexes.CreateMany([uniqueIndex, ttlIndex]);
    }
}
