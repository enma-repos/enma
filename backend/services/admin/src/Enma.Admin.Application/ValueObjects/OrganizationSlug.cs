using Enma.Common.Constants;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.ValueObjects;

public readonly record struct OrganizationSlug(string Value)
{
    public static Result<OrganizationSlug> Create(string? slug)
    {
        slug = (slug ?? string.Empty).Trim().ToLowerInvariant();

        if (slug.Length is < 3 or > 64)
        {
            return Result.Fail<OrganizationSlug>(ApplicationErrors.Length("Slug", 3, 64));
        }

        if (!RegexPatterns.KebabCase().IsMatch(slug))
        {
            return Result.Fail<OrganizationSlug>(ApplicationErrors.Validation("Slug must match [a-z0-9-] pattern."));
        }

        return Result.Ok(new OrganizationSlug(slug));
    }

    public override string ToString() => Value;
}
