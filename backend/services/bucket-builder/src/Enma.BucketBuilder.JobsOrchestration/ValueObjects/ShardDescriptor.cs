using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.JobsOrchestration.ValueObjects;

/// <summary>
/// Execution shard descriptor used to parallelize processing while preserving per-project ordering.
/// </summary>
public readonly record struct ShardDescriptor
{
    public int Index { get; }
    public int Count { get; }

    private ShardDescriptor(int index, int count)
    {
        Index = index;
        Count = count;
    }

    public static Result<ShardDescriptor> Create(int index, int count)
    {
        var errors = new List<IError>();

        if (count <= 0)
        {
            errors.Add(ApplicationErrors.Validation("count must be greater than zero."));
        }

        if (index < 0)
        {
            errors.Add(ApplicationErrors.Validation("index must be non-negative."));
        }

        if (count > 0 && index >= count)
        {
            errors.Add(ApplicationErrors.Validation("index must be less than count."));
        }

        return errors.Count > 0
            ? Result.Fail<ShardDescriptor>(errors)
            : Result.Ok(new ShardDescriptor(index, count));
    }

    public static ShardDescriptor Rehydrate(int index, int count) => new(index, count);
}
