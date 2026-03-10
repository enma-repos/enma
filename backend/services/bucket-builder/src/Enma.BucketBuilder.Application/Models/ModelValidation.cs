using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

internal static class ModelValidation
{
    public static void AddRequiredGuid(List<IError> errors, Guid value, string fieldName)
    {
        if (value == Guid.Empty)
        {
            errors.Add(ApplicationErrors.Required(fieldName));
        }
    }

    public static void AddUtcDateTime(List<IError> errors, DateTime value, string fieldName)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation($"{fieldName} must be UTC."));
        }
    }

    public static string ValidateRequiredString(
        List<IError> errors,
        string? value,
        string fieldName,
        int minLength,
        int maxLength)
    {
        var normalized = (value ?? string.Empty).Trim();

        if (normalized.Length < minLength || normalized.Length > maxLength)
        {
            errors.Add(ApplicationErrors.Length(fieldName, minLength, maxLength));
        }

        return normalized;
    }

    public static string? ValidateOptionalString(
        List<IError> errors,
        string? value,
        string fieldName,
        int maxLength)
    {
        if (value is null)
        {
            return null;
        }

        var normalized = value.Trim();
        if (normalized.Length == 0)
        {
            return null;
        }

        if (normalized.Length > maxLength)
        {
            errors.Add(ApplicationErrors.Length(fieldName, 0, maxLength));
        }

        return normalized;
    }

    public static void AddNonNegativeLong(List<IError> errors, long value, string fieldName)
    {
        if (value < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{fieldName} must be non-negative."));
        }
    }

    public static void AddNonNegativeInt(List<IError> errors, int value, string fieldName)
    {
        if (value < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{fieldName} must be non-negative."));
        }
    }

    public static bool IsMinuteAligned(DateTime value)
    {
        return value.Second == 0 && value.Millisecond == 0 && value.Microsecond == 0;
    }

    public static bool IsFiveMinuteBoundary(DateTime value)
    {
        return IsMinuteAligned(value) && value.Minute % 5 == 0;
    }
}
