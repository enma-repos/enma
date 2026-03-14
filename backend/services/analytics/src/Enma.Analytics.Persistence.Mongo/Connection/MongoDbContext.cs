using Enma.Analytics.Persistence.Mongo.Documents;
using Enma.Common.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Enma.Analytics.Persistence.Mongo.Connection;

internal sealed class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public IMongoCollection<PathNodeBucketDocument> PathNodeBuckets
        => _database.GetCollection<PathNodeBucketDocument>("path_node_buckets");

    public IMongoCollection<PathEdgeBucketDocument> PathEdgeBuckets
        => _database.GetCollection<PathEdgeBucketDocument>("path_edge_buckets");

    public MongoDbContext(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }
}
