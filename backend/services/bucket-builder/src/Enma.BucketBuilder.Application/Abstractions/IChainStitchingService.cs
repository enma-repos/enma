using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using FluentResults;

namespace Enma.BucketBuilder.Application.Abstractions;

internal interface IChainStitchingService
{
    Result<ChainAggregationSlice> BuildSlice(
        BucketWindow? window,
        ChainCursor? existingCursor,
        IReadOnlyList<PathSourceEvent>? orderedEvents);
}
