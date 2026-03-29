namespace Enma.Analytics.Application.Models;

public sealed record NodeSummary(
    long TotalVisits,
    long TotalEntries,
    long TotalExits,
    long TotalUniqueChains);
