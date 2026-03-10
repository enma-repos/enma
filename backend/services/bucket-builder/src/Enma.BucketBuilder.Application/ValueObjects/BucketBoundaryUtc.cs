using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

public readonly record struct BucketBoundaryUtc(DateTime Value)
{
    public static Result<BucketBoundaryUtc> Create(DateTime value)
    {
        var errors = new List<IError>();

        if (value.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation("Bucket boundary must be UTC."));
        }

        if (value.Second != 0 || value.Millisecond != 0 || value.Microsecond != 0)
        {
            errors.Add(ApplicationErrors.Validation("Bucket boundary must be minute-aligned."));
        }

        if (value.Minute % 5 != 0)
        {
            errors.Add(ApplicationErrors.Validation("Bucket boundary must be aligned to 5-minute interval."));
        }

        return errors.Count > 0
            ? Result.Fail<BucketBoundaryUtc>(errors)
            : Result.Ok(new BucketBoundaryUtc(value));
    }

    public static BucketBoundaryUtc Rehydrate(DateTime value)
        => new(value);

    public override string ToString() => Value.ToString("O");
}
