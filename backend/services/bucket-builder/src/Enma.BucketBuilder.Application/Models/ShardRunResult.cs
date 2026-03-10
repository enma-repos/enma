using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

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
        ShardDescriptor? shard,
        int windowsProcessed,
        DateTime? fromCheckpointUtc,
        DateTime? toCheckpointUtc,
        long totalEventsRead,
        long totalDurationMs)
    {
        var errors = new List<IError>();

        if (shard is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(shard)));
        }

        if (windowsProcessed < 0)
        {
            errors.Add(ApplicationErrors.Validation("windowsProcessed must be non-negative."));
        }

        if (fromCheckpointUtc.HasValue)
        {
            ModelValidation.AddUtcDateTime(errors, fromCheckpointUtc.Value, nameof(fromCheckpointUtc));
        }

        if (toCheckpointUtc.HasValue)
        {
            ModelValidation.AddUtcDateTime(errors, toCheckpointUtc.Value, nameof(toCheckpointUtc));
        }

        if (fromCheckpointUtc.HasValue && toCheckpointUtc.HasValue && toCheckpointUtc < fromCheckpointUtc)
        {
            errors.Add(ApplicationErrors.Validation("toCheckpointUtc cannot be earlier than fromCheckpointUtc."));
        }

        ModelValidation.AddNonNegativeLong(errors, totalEventsRead, nameof(totalEventsRead));
        ModelValidation.AddNonNegativeLong(errors, totalDurationMs, nameof(totalDurationMs));

        return errors.Count > 0
            ? Result.Fail<ShardRunResult>(errors)
            : Result.Ok(new ShardRunResult(
                shard!,
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