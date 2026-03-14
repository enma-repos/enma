using Enma.Common.Errors;
using FluentResults;

namespace Enma.Analytics.Application.ValueObjects;

public sealed record DateRange
{
    private static readonly TimeSpan MaxRange = TimeSpan.FromDays(90);

    public DateTime FromUtc { get; }
    public DateTime ToUtc { get; }

    private DateRange(DateTime fromUtc, DateTime toUtc)
    {
        FromUtc = fromUtc;
        ToUtc = toUtc;
    }

    public static Result<DateRange> Create(DateTime? from, DateTime? to)
    {
        if (from is null)
            return Result.Fail<DateRange>(ApplicationErrors.Required("from"));

        if (to is null)
            return Result.Fail<DateRange>(ApplicationErrors.Required("to"));

        if (from.Value.Kind != DateTimeKind.Utc)
            return Result.Fail<DateRange>(ApplicationErrors.Validation("'from' must be a UTC timestamp."));

        if (to.Value.Kind != DateTimeKind.Utc)
            return Result.Fail<DateRange>(ApplicationErrors.Validation("'to' must be a UTC timestamp."));

        if (from.Value >= to.Value)
            return Result.Fail<DateRange>(ApplicationErrors.Validation("'from' must be earlier than 'to'."));

        if (to.Value - from.Value > MaxRange)
            return Result.Fail<DateRange>(ApplicationErrors.Validation($"Date range must not exceed {MaxRange.Days} days."));

        return Result.Ok(new DateRange(from.Value, to.Value));
    }
}
