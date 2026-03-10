using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

public readonly record struct EventName(string Value)
{
    public static Result<EventName> Create(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();

        if (normalized.Length is < 1 or > 200)
        {
            return Result.Fail<EventName>(ApplicationErrors.Length(nameof(EventName), 1, 200));
        }

        return Result.Ok(new EventName(normalized));
    }

    public static EventName Rehydrate(string value)
        => new(value);

    public override string ToString() => Value;
}
