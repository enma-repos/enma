using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

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
        ShardDescriptor? shard,
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

        if (shard is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(shard)));
        }

        ModelValidation.AddNonNegativeLong(errors, eventsRead, nameof(eventsRead));
        ModelValidation.AddNonNegativeLong(errors, distinctChains, nameof(distinctChains));
        ModelValidation.AddNonNegativeLong(errors, edgesUpserted, nameof(edgesUpserted));
        ModelValidation.AddNonNegativeLong(errors, nodesUpserted, nameof(nodesUpserted));
        ModelValidation.AddNonNegativeLong(errors, cursorsUpserted, nameof(cursorsUpserted));
        ModelValidation.AddNonNegativeLong(errors, durationMs, nameof(durationMs));

        return errors.Count > 0
            ? Result.Fail<WindowBuildResult>(errors)
            : Result.Ok(new WindowBuildResult(
                window!,
                shard!,
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