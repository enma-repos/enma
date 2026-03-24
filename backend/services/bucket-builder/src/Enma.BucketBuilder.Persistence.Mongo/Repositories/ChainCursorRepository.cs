using Enma.BucketBuilder.Application.Contracts.Persistence;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using Enma.BucketBuilder.Persistence.Mongo.Connection;
using Enma.BucketBuilder.Persistence.Mongo.Documents;
using FluentResults;
using MongoDB.Driver;

namespace Enma.BucketBuilder.Persistence.Mongo.Repositories;

internal sealed class ChainCursorRepository : IChainCursorRepository
{
    private const int OrBatchSize = 100;
    private readonly IMongoCollection<ChainCursorDocument> _collection;

    public ChainCursorRepository(IMongoDbContext context)
    {
        _collection = context.ChainCursors;
    }

    public async Task<Result<IReadOnlyDictionary<ChainKey, ChainCursor>>> GetByChainKeysAsync(
        IReadOnlyCollection<ChainKey> chainKeys,
        CancellationToken ct = default)
    {
        if (chainKeys.Count == 0)
        {
            return Result.Ok<IReadOnlyDictionary<ChainKey, ChainCursor>>(
                new Dictionary<ChainKey, ChainCursor>());
        }

        var allDocs = new List<ChainCursorDocument>();
        var chainKeysList = chainKeys as IList<ChainKey> ?? chainKeys.ToArray();

        for (var i = 0; i < chainKeysList.Count; i += OrBatchSize)
        {
            var batchSize = Math.Min(OrBatchSize, chainKeysList.Count - i);
            var orFilters = new List<FilterDefinition<ChainCursorDocument>>(batchSize);

            for (var j = i; j < i + batchSize; j++)
            {
                var ck = chainKeysList[j];
                orFilters.Add(Builders<ChainCursorDocument>.Filter.And(
                    Builders<ChainCursorDocument>.Filter.Eq(d => d.OrganizationId, ck.OrganizationId),
                    Builders<ChainCursorDocument>.Filter.Eq(d => d.ProjectId, ck.ProjectId),
                    Builders<ChainCursorDocument>.Filter.Eq(d => d.ProcessDefinitionId, ck.ProcessDefinitionId),
                    Builders<ChainCursorDocument>.Filter.Eq(d => d.ProcessId, ck.ProcessId.Value)));
            }

            var filter = Builders<ChainCursorDocument>.Filter.Or(orFilters);
            var docs = await _collection.Find(filter).ToListAsync(ct);
            allDocs.AddRange(docs);
        }

        var result = new Dictionary<ChainKey, ChainCursor>(allDocs.Count);

        foreach (var doc in allDocs)
        {
            var chainKey = ChainKey.Rehydrate(
                doc.OrganizationId, doc.ProjectId, doc.ProcessDefinitionId, doc.ProcessId);

            var cursor = ChainCursor.Rehydrate(
                chainKey,
                doc.LastEventId,
                doc.LastEventName,
                DateTime.SpecifyKind(doc.LastOccurredAtUtc, DateTimeKind.Utc),
                doc.LastActorUserId,
                doc.LastActorAnonymousId,
                DateTime.SpecifyKind(doc.UpdatedAtUtc, DateTimeKind.Utc),
                doc.FirstEventName);

            result[chainKey] = cursor;
        }

        return Result.Ok<IReadOnlyDictionary<ChainKey, ChainCursor>>(result);
    }

    public async Task<Result> UpsertBatchAsync(IReadOnlyCollection<ChainCursor> cursors, CancellationToken ct = default)
    {
        if (cursors.Count == 0) return Result.Ok();

        var models = new List<WriteModel<ChainCursorDocument>>(cursors.Count);

        foreach (var cursor in cursors)
        {
            var filter = Builders<ChainCursorDocument>.Filter.And(
                Builders<ChainCursorDocument>.Filter.Eq(d => d.OrganizationId, cursor.ChainKey.OrganizationId),
                Builders<ChainCursorDocument>.Filter.Eq(d => d.ProjectId, cursor.ChainKey.ProjectId),
                Builders<ChainCursorDocument>.Filter.Eq(d => d.ProcessDefinitionId, cursor.ChainKey.ProcessDefinitionId),
                Builders<ChainCursorDocument>.Filter.Eq(d => d.ProcessId, cursor.ChainKey.ProcessId.Value));

            var doc = new ChainCursorDocument
            {
                OrganizationId = cursor.ChainKey.OrganizationId,
                ProjectId = cursor.ChainKey.ProjectId,
                ProcessDefinitionId = cursor.ChainKey.ProcessDefinitionId,
                ProcessId = cursor.ChainKey.ProcessId.Value,
                LastEventId = cursor.LastEventId,
                LastEventName = cursor.LastEventName.Value,
                LastOccurredAtUtc = cursor.LastOccurredAtUtc,
                LastActorUserId = cursor.LastActorUserId?.Value,
                LastActorAnonymousId = cursor.LastActorAnonymousId?.Value,
                UpdatedAtUtc = cursor.UpdatedAtUtc,
                FirstEventName = cursor.FirstEventName.Value
            };

            models.Add(new ReplaceOneModel<ChainCursorDocument>(filter, doc) { IsUpsert = true });
        }

        await _collection.BulkWriteAsync(models, cancellationToken: ct);
        return Result.Ok();
    }
}
