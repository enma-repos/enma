using Enma.Analytics.Application.Enums;
using Enma.Analytics.Application.Models;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Contracts;

public interface IPathEdgeQueryRepository
{
    Task<Result<IReadOnlyList<AggregatedEdge>>> GetAggregatedEdgesAsync(
        ProcessFilter filter, CancellationToken ct = default);

    Task<Result<IReadOnlyList<EdgeTimeSeries>>> GetEdgeTimeSeriesAsync(
        ProcessFilter filter, Granularity granularity, CancellationToken ct = default);

    Task<Result<IReadOnlyList<AggregatedEdge>>> GetEdgesForEventAsync(
        ProcessFilter filter, string eventName, CancellationToken ct = default);

    Task<Result<IReadOnlyList<AggregatedEdge>>> GetAggregatedEdgesByEntryEventAsync(
        ProcessFilter filter, string entryEventName, CancellationToken ct = default);

    Task<Result<IReadOnlyList<AggregatedEdge>>> GetAggregatedEdgesAsync(
        MultiProcessFilter filter, CancellationToken ct = default);
}
