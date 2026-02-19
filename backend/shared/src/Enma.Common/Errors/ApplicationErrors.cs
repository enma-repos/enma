using Enma.Common.Errors.Types;

namespace Enma.Common.Errors;

public static class ApplicationErrors
{
    public static ValidationError Validation(string message, string code = "validation")
        => new(message, code);

    public static NotFoundError NotFound(string message, string code = "not_found")
        => new(message, code);

    public static ConflictError Conflict(string message, string code = "conflict")
        => new(message, code);

    public static ForbiddenError Forbidden(string message, string code = "forbidden")
        => new(message, code);

    public static ValidationError Required(string field)
        => new($"{field} is required.", code: "required");

    public static ValidationError Length(string field, int min, int max)
        => new($"{field} length must be {min}..{max}.", code: "length");

    public static ValidationError InvalidFormat(string field)
        => new($"{field} has invalid format.", code: "format");

    public static NotFoundError EntityNotFound(string entity, string? hint = null)
        => new($"{entity} not found{(hint is null ? "" : $": {hint}")}.",
            code: $"{entity.ToLowerInvariant()}_not_found");

    public static ConflictError AlreadyExists(string entity, string? hint = null)
        => new($"{entity} already exists{(hint is null ? "" : $": {hint}")}.",
            code: $"{entity.ToLowerInvariant()}_exists");

    public static ConflictError InvariantViolation(string message)
        => new(message, code: "invariant");
}