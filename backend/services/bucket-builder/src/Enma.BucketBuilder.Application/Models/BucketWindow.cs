using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Fixed 5-minute UTC window used as a deterministic unit of bucket calculation.
/// </summary>
public sealed class BucketWindow
{
    private static readonly TimeSpan BucketSize = TimeSpan.FromMinutes(5);

    public DateTime StartUtc { get; }
    public DateTime EndUtc { get; }

    private BucketWindow(DateTime startUtc, DateTime endUtc)
    {
        StartUtc = startUtc;
        EndUtc = endUtc;
    }

    public static Result<BucketWindow> Create(DateTime startUtc, DateTime endUtc)
    {
        var errors = new List<IError>();

        ModelValidation.AddUtcDateTime(errors, startUtc, nameof(startUtc));
        ModelValidation.AddUtcDateTime(errors, endUtc, nameof(endUtc));

        if (endUtc <= startUtc)
        {
            errors.Add(ApplicationErrors.Validation("endUtc must be greater than startUtc."));
        }

        if (endUtc - startUtc != BucketSize)
        {
            errors.Add(ApplicationErrors.Validation("Bucket window must be exactly 5 minutes."));
        }

        if (!ModelValidation.IsFiveMinuteBoundary(startUtc))
        {
            errors.Add(ApplicationErrors.Validation("startUtc must be aligned to a 5-minute boundary."));
        }

        if (!ModelValidation.IsFiveMinuteBoundary(endUtc))
        {
            errors.Add(ApplicationErrors.Validation("endUtc must be aligned to a 5-minute boundary."));
        }

        return errors.Count > 0
            ? Result.Fail<BucketWindow>(errors)
            : Result.Ok(new BucketWindow(startUtc, endUtc));
    }

    public static BucketWindow Rehydrate(DateTime startUtc, DateTime endUtc) => new (startUtc, endUtc);
}