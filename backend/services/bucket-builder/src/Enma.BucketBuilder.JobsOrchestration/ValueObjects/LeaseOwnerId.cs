using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.JobsOrchestration.ValueObjects;

public sealed record LeaseOwnerId
{
    public string Value { get; }

    private LeaseOwnerId(string value)
    {
        Value = value;
    }

    public static Result<LeaseOwnerId> Create(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();

        if (normalized.Length is < 1 or > 200)
        {
            return Result.Fail<LeaseOwnerId>(ApplicationErrors.Length(nameof(LeaseOwnerId), 1, 200));
        }

        return Result.Ok(new LeaseOwnerId(normalized));
    }

    public static Result<LeaseOwnerId?> CreateOptional(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Ok<LeaseOwnerId?>(null);
        }

        var result = Create(value);
        return result.IsSuccess
            ? Result.Ok<LeaseOwnerId?>(result.Value)
            : Result.Fail<LeaseOwnerId?>(result.Errors);
    }

    public static LeaseOwnerId Rehydrate(string value)
        => new(value);

    public override string ToString() => Value;
}
