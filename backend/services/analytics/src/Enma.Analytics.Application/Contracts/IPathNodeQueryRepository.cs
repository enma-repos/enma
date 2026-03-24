using Enma.Analytics.Application.Enums;
using Enma.Analytics.Application.Models;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Contracts;

public interface IPathNodeQueryRepository
{
    Task<Result<IReadOnlyList<AggregatedNode>>> GetAggregatedNodesAsync(
        ProcessFilter filter, CancellationToken ct = default);

    Task<Result<IReadOnlyList<NodeTimeSeries>>> GetNodeTimeSeriesAsync(
        ProcessFilter filter, Granularity granularity, CancellationToken ct = default);

    Task<Result<AggregatedNode?>> GetSingleNodeAsync(
        ProcessFilter filter, string eventName, CancellationToken ct = default);

    Task<Result<IReadOnlyList<AggregatedNode>>> GetNodesForEventsAsync(
        ProcessFilter filter, IReadOnlyList<string> eventNames, CancellationToken ct = default);

    Task<Result<IReadOnlyList<AggregatedNode>>> GetAggregatedNodesByEntryEventAsync(
        ProcessFilter filter, string entryEventName, CancellationToken ct = default);

    Task<Result<IReadOnlyList<AggregatedNode>>> GetAggregatedNodesAsync(
        MultiProcessFilter filter, CancellationToken ct = default);
}
