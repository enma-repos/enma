using System.Text.RegularExpressions;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.ValueTypes;

public readonly record struct ProjectKey(string Value)
{
    private static readonly Regex Rx = new("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);
    
    public static Result<ProjectKey> Create(string? key)
    {
        key = (key ?? string.Empty).Trim().ToLowerInvariant();

        if (key.Length is < 2 or > 64)
        {
            return Result.Fail<ProjectKey>(ApplicationErrors.Length("Project key", 2, 64));
        }

        if (!Rx.IsMatch(key))
        {
            return Result.Fail<ProjectKey>(ApplicationErrors.Validation("Project key must match [a-z0-9-] pattern."));
        }
        
        return Result.Ok(new ProjectKey(key));
    }

    public override string ToString() => Value;
}