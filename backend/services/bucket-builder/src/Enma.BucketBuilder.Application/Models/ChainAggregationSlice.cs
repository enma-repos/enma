namespace Enma.BucketBuilder.Application.Models;

internal sealed record ChainAggregationSlice(
    IReadOnlyCollection<NodeVisit> NodeVisits,
    IReadOnlyCollection<EdgeTransition> EdgeTransitions,
    ChainCursor? UpdatedCursor)
{
    public static ChainAggregationSlice Empty { get; } = new(
        Array.Empty<NodeVisit>(),
        Array.Empty<EdgeTransition>(),
        null);
}
