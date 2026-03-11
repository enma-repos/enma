using Enma.BucketBuilder.JobsOrchestration.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.JobsOrchestration.Models;

/// <summary>
/// Execution summary for one shard job run.
/// </summary>
public sealed class ShardRunResult
{
    public ShardDescriptor Shard { get; }
    public int WindowsProcessed { get; }
    public DateTime? FromCheckpointUtc { get; }
    public DateTime? ToCheckpointUtc { get; }
    public long TotalEventsRead { get; }
    public long TotalDurationMs { get; }

    private ShardRunResult(
        ShardDescriptor shard,
        int windowsProcessed,
        DateTime? fromCheckpointUtc,
        DateTime? toCheckpointUtc,
        long totalEventsRead,
        long totalDurationMs)
    {
        Shard = shard;
        WindowsProcessed = windowsProcessed;
        FromCheckpointUtc = fromCheckpointUtc;
        ToCheckpointUtc = toCheckpointUtc;
        TotalEventsRead = totalEventsRead;
        TotalDurationMs = totalDurationMs;
    }

    public static Result<ShardRunResult> Create(
        ShardDescriptor shard,
        int windowsProcessed,
        DateTime? fromCheckpointUtc,
        DateTime? toCheckpointUtc,
        long totalEventsRead,
        long totalDurationMs)
    {
        var errors = new List<IError>();

        if (windowsProcessed < 0)
        {
            errors.Add(ApplicationErrors.Validation("windowsProcessed must be non-negative."));
        }

        if (fromCheckpointUtc.HasValue)
        {
            if (fromCheckpointUtc.Value.Kind != DateTimeKind.Utc)
            {
                errors.Add(ApplicationErrors.Validation($"{nameof(fromCheckpointUtc)} must be UTC when provided."));
            }
        }

        if (toCheckpointUtc.HasValue)
        {
            if (toCheckpointUtc.Value.Kind != DateTimeKind.Utc)
            {
                errors.Add(ApplicationErrors.Validation($"{nameof(toCheckpointUtc)} must be UTC when provided."));
            }
        }

        if (fromCheckpointUtc.HasValue && toCheckpointUtc.HasValue && toCheckpointUtc < fromCheckpointUtc)
        {
            errors.Add(ApplicationErrors.Validation("toCheckpointUtc cannot be earlier than fromCheckpointUtc."));
        }

        if (totalEventsRead < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(totalEventsRead)} must be non-negative."));
        }

        if (totalDurationMs < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(totalDurationMs)} must be non-negative."));
        }

        return errors.Count > 0
            ? Result.Fail<ShardRunResult>(errors)
            : Result.Ok(new ShardRunResult(
                shard,
                windowsProcessed,
                fromCheckpointUtc,
                toCheckpointUtc,
                totalEventsRead,
                totalDurationMs));
    }

    public static ShardRunResult Rehydrate(
        ShardDescriptor shard,
        int windowsProcessed,
        DateTime? fromCheckpointUtc,
        DateTime? toCheckpointUtc,
        long totalEventsRead,
        long totalDurationMs)
        => new(
            shard,
            windowsProcessed,
            fromCheckpointUtc,
            toCheckpointUtc,
            totalEventsRead,
            totalDurationMs);
}
