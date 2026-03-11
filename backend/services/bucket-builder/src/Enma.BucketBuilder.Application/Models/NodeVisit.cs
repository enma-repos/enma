using Enma.BucketBuilder.Application.ValueObjects;

namespace Enma.BucketBuilder.Application.Models;

internal sealed record NodeVisit(
    ChainKey ChainKey,
    NodeKey Key,
    bool IsEntry,
    bool IsExit);
