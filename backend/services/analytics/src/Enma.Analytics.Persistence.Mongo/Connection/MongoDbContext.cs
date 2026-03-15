using Enma.Analytics.Persistence.Mongo.Documents;
using Enma.Common.Options;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Enma.Analytics.Persistence.Mongo.Connection;

internal sealed class MongoDbContext : IMongoDbContext
{
    private static readonly object _initLock = new();
    private static bool _serializersRegistered;

    private readonly IMongoDatabase _database;

    public IMongoCollection<PathNodeBucketDocument> PathNodeBuckets
        => _database.GetCollection<PathNodeBucketDocument>("path_node_buckets");

    public IMongoCollection<PathEdgeBucketDocument> PathEdgeBuckets
        => _database.GetCollection<PathEdgeBucketDocument>("path_edge_buckets");

    public MongoDbContext(IOptions<MongoDbOptions> options)
    {
        RegisterSerializers();

        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    private static void RegisterSerializers()
    {
        lock (_initLock)
        {
            if (_serializersRegistered) return;
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            _serializersRegistered = true;
        }
    }
}
