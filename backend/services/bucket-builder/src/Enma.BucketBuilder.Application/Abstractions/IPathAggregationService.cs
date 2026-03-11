using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using FluentResults;

namespace Enma.BucketBuilder.Application.Abstractions;

internal interface IPathAggregationService
{
    Result<WindowProjectionBatch> Aggregate(
        BucketWindow? window,
        IReadOnlyCollection<PathSourceEvent>? sourceEvents,
        IReadOnlyDictionary<ChainKey, ChainCursor>? existingCursors);
}
