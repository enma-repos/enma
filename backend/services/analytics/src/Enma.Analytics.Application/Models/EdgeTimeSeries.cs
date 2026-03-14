namespace Enma.Analytics.Application.Models;

public sealed record EdgeTimeSeries(
    DateTime BucketStart,
    long TotalTransitions,
    long TotalUniqueChains,
    long TotalUniqueUsers,
    long TotalUniqueAnonymous);
