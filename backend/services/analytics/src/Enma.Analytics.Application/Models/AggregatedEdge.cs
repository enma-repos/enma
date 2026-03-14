namespace Enma.Analytics.Application.Models;

public sealed record AggregatedEdge(
    string FromEvent,
    string ToEvent,
    long TotalTransitions,
    long TotalUniqueChains,
    long TotalUniqueUsers,
    long TotalUniqueAnonymous);
