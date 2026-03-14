using Enma.Analytics.Persistence.Mongo.Documents;
using MongoDB.Driver;

namespace Enma.Analytics.Persistence.Mongo.Connection;

internal interface IMongoDbContext
{
    IMongoCollection<PathNodeBucketDocument> PathNodeBuckets { get; }
    IMongoCollection<PathEdgeBucketDocument> PathEdgeBuckets { get; }
}
