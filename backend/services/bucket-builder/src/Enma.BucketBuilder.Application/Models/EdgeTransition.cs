using Enma.BucketBuilder.Application.ValueObjects;

namespace Enma.BucketBuilder.Application.Models;

internal sealed record EdgeTransition(
    ChainKey ChainKey,
    EdgeKey Key,
    ActorIdentifier? ActorUserId,
    ActorIdentifier? ActorAnonymousId);
