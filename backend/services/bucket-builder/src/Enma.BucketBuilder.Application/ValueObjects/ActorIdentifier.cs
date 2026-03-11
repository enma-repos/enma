using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

public sealed record ActorIdentifier
{
    public string Value { get; }

    private ActorIdentifier(string value)
    {
        Value = value;
    }

    public static Result<ActorIdentifier> Create(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();

        if (normalized.Length is < 1 or > 256)
        {
            return Result.Fail<ActorIdentifier>(ApplicationErrors.Length(nameof(ActorIdentifier), 1, 256));
        }

        return Result.Ok(new ActorIdentifier(normalized));
    }

    public static Result<ActorIdentifier?> CreateOptional(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Ok<ActorIdentifier?>(null);
        }

        var result = Create(value);
        return result.IsSuccess
            ? Result.Ok<ActorIdentifier?>(result.Value)
            : Result.Fail<ActorIdentifier?>(result.Errors);
    }

    public static ActorIdentifier Rehydrate(string value)
        => new(value);

    public override string ToString() => Value;
}
