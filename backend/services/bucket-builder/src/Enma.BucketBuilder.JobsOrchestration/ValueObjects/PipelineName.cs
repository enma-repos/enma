using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.JobsOrchestration.ValueObjects;

public sealed record PipelineName
{
    public string Value { get; }

    private PipelineName(string value)
    {
        Value = value;
    }

    public static Result<PipelineName> Create(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();

        if (normalized.Length is < 1 or > 64)
        {
            return Result.Fail<PipelineName>(ApplicationErrors.Length(nameof(PipelineName), 1, 64));
        }

        return Result.Ok(new PipelineName(normalized));
    }

    public static PipelineName Rehydrate(string value)
        => new(value);

    public override string ToString() => Value;
}
