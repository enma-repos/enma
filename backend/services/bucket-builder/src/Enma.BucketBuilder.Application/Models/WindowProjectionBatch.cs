using Enma.BucketBuilder.Application.ValueObjects;

namespace Enma.BucketBuilder.Application.Models;

public sealed record WindowProjectionBatch(
    BucketWindow Window,
    long EventsRead,
    long DistinctChains,
    IReadOnlyCollection<PathNodeBucket> NodeBuckets,
    IReadOnlyCollection<PathEdgeBucket> EdgeBuckets,
    IReadOnlyCollection<ChainCursor> UpdatedCursors)
{
    public static WindowProjectionBatch Empty(BucketWindow window)
        => new(
            window,
            0,
            0,
            Array.Empty<PathNodeBucket>(),
            Array.Empty<PathEdgeBucket>(),
            Array.Empty<ChainCursor>());
}
