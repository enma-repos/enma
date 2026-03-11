using Enma.BucketBuilder.Application.ValueObjects;
using Enma.BucketBuilder.JobsOrchestration.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.JobsOrchestration.Models;

/// <summary>
/// Immutable execution metrics for a single processed bucket window.
/// </summary>
public sealed class WindowBuildResult
{
    public BucketWindow Window { get; }
    public ShardDescriptor Shard { get; }
    public long EventsRead { get; }
    public long DistinctChains { get; }
    public long EdgesUpserted { get; }
    public long NodesUpserted { get; }
    public long CursorsUpserted { get; }
    public long DurationMs { get; }

    private WindowBuildResult(
        BucketWindow window,
        ShardDescriptor shard,
        long eventsRead,
        long distinctChains,
        long edgesUpserted,
        long nodesUpserted,
        long cursorsUpserted,
        long durationMs)
    {
        Window = window;
        Shard = shard;
        EventsRead = eventsRead;
        DistinctChains = distinctChains;
        EdgesUpserted = edgesUpserted;
        NodesUpserted = nodesUpserted;
        CursorsUpserted = cursorsUpserted;
        DurationMs = durationMs;
    }

    public static Result<WindowBuildResult> Create(
        BucketWindow? window,
        ShardDescriptor shard,
        long eventsRead,
        long distinctChains,
        long edgesUpserted,
        long nodesUpserted,
        long cursorsUpserted,
        long durationMs)
    {
        var errors = new List<IError>();

        if (window is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(window)));
        }

        if (eventsRead < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(eventsRead)} must be non-negative."));
        }

        if (distinctChains < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(distinctChains)} must be non-negative."));
        }

        if (edgesUpserted < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(edgesUpserted)} must be non-negative."));
        }

        if (nodesUpserted < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(nodesUpserted)} must be non-negative."));
        }

        if (cursorsUpserted < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(cursorsUpserted)} must be non-negative."));
        }

        if (durationMs < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(durationMs)} must be non-negative."));
        }

        return errors.Count > 0
            ? Result.Fail<WindowBuildResult>(errors)
            : Result.Ok(new WindowBuildResult(
                window!,
                shard,
                eventsRead,
                distinctChains,
                edgesUpserted,
                nodesUpserted,
                cursorsUpserted,
                durationMs));
    }

    public static WindowBuildResult Rehydrate(
        BucketWindow window,
        ShardDescriptor shard,
        long eventsRead,
        long distinctChains,
        long edgesUpserted,
        long nodesUpserted,
        long cursorsUpserted,
        long durationMs)
        => new(
            window,
            shard,
            eventsRead,
            distinctChains,
            edgesUpserted,
            nodesUpserted,
            cursorsUpserted,
            durationMs);
}
