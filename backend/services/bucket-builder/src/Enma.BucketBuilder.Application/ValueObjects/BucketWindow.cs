using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

/// <summary>
/// Fixed 5-minute UTC window used as a deterministic unit of bucket calculation.
/// </summary>
public readonly record struct BucketWindow(BucketBoundaryUtc StartUtc, BucketBoundaryUtc EndUtc)
{
    private static readonly TimeSpan BucketSize = TimeSpan.FromMinutes(5);

    public static Result<BucketWindow> Create(DateTime startUtc, DateTime endUtc)
    {
        var errors = new List<IError>();

        var startVoResult = BucketBoundaryUtc.Create(startUtc);
        if (startVoResult.IsFailed)
        {
            errors.AddRange(startVoResult.Errors);
        }

        var endVoResult = BucketBoundaryUtc.Create(endUtc);
        if (endVoResult.IsFailed)
        {
            errors.AddRange(endVoResult.Errors);
        }

        if (endUtc <= startUtc)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(endUtc)} must be greater than {nameof(startUtc)}."));
        }

        if (endUtc - startUtc != BucketSize)
        {
            errors.Add(ApplicationErrors.Validation("Bucket window must be exactly 5 minutes."));
        }

        return errors.Count > 0
            ? Result.Fail<BucketWindow>(errors)
            : Result.Ok(new BucketWindow(startVoResult.Value, endVoResult.Value));
    }

    public static BucketWindow Rehydrate(DateTime startUtc, DateTime endUtc)
        => new(BucketBoundaryUtc.Rehydrate(startUtc), BucketBoundaryUtc.Rehydrate(endUtc));
}
