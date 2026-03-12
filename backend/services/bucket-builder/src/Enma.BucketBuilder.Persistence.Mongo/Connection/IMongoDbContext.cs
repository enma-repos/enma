using Enma.BucketBuilder.Persistence.Mongo.Documents;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Connection;

internal interface IMongoDbContext
{
    IMongoCollection<PathNodeBucketDocument> PathNodeBuckets { get; }
    IMongoCollection<PathEdgeBucketDocument> PathEdgeBuckets { get; }
    IMongoCollection<ChainCursorDocument> ChainCursors { get; }
    IMongoCollection<ShardCheckpointDocument> ShardCheckpoints { get; }
}
