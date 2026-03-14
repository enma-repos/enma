namespace Enma.Analytics.Application.Models;

public sealed record AggregatedNode(
    string EventName,
    long TotalVisits,
    long TotalEntries,
    long TotalExits,
    long TotalUniqueChains);
