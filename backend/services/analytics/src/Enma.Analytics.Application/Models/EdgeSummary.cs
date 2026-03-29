namespace Enma.Analytics.Application.Models;

public sealed record EdgeSummary(
    long TotalTransitions,
    long TotalUniqueUsers,
    long TotalUniqueAnonymous);
