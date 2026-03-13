using Enma.Common.Constants;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.ValueObjects;

public readonly record struct ProjectKey(string Value)
{
    public static Result<ProjectKey> Create(string? key)
    {
        key = (key ?? string.Empty).Trim().ToLowerInvariant();

        if (key.Length is < 2 or > 64)
        {
            return Result.Fail<ProjectKey>(ApplicationErrors.Length("Project key", 2, 64));
        }

        if (!RegexPatterns.KebabCase().IsMatch(key))
        {
            return Result.Fail<ProjectKey>(ApplicationErrors.Validation("Project key must match [a-z0-9-] pattern."));
        }

        return Result.Ok(new ProjectKey(key));
    }

    public override string ToString() => Value;
}
