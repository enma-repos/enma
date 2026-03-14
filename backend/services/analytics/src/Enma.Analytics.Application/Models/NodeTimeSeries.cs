namespace Enma.Analytics.Application.Models;

public sealed record NodeTimeSeries(
    DateTime BucketStart,
    long TotalVisits,
    long TotalEntries,
    long TotalExits,
    long TotalUniqueChains);
