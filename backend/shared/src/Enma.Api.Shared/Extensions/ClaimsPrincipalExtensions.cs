using System.Security.Claims;
using Enma.Common.Constants;
using Enma.Common.Enums;

namespace Enma.Api.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetAccountId(this ClaimsPrincipal user, out Guid accountId)
    {
        var claim = user.FindFirst("accountId")?.Value;
        return Guid.TryParse(claim, out accountId);
    }

    public static bool TryGetRole(this ClaimsPrincipal user, out UserRole role)
    {
        role = UserRole.Member;
        var raw = user.FindFirst(AuthorizationPolicies.RoleClaimType)?.Value;
        return !string.IsNullOrWhiteSpace(raw) && Enum.TryParse(raw, ignoreCase: true, out role);
    }

    public static bool IsSuperAdmin(this ClaimsPrincipal user)
        => user.TryGetRole(out var r) && r == UserRole.SuperAdmin;
}
