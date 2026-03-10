using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

public readonly record struct ProcessId(string Value)
{
    public static Result<ProcessId> Create(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();

        if (normalized.Length is < 1 or > 256)
        {
            return Result.Fail<ProcessId>(ApplicationErrors.Length(nameof(ProcessId), 1, 256));
        }

        return Result.Ok(new ProcessId(normalized));
    }

    public static ProcessId Rehydrate(string value) => new(value);
    public override string ToString() => Value;
}
