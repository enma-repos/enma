using Enma.Common.Constants;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.ValueObjects;

public readonly record struct ProcessDefinitionKey(string Value)
{
    public static Result<ProcessDefinitionKey> Create(string? key)
    {
        key = (key ?? string.Empty).Trim().ToLowerInvariant();

        if (key.Length is < 2 or > 64)
        {
            return Result.Fail<ProcessDefinitionKey>(ApplicationErrors.Length("Process definition key", 2, 64));
        }

        if (!RegexPatterns.KebabCase().IsMatch(key))
        {
            return Result.Fail<ProcessDefinitionKey>(
                ApplicationErrors.Validation("Process definition key must match [a-z0-9-] pattern."));
        }

        return Result.Ok(new ProcessDefinitionKey(key));
    }

    public override string ToString() => Value;
}
