using System.Security.Claims;

namespace Enma.Api.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetAccountId(this ClaimsPrincipal user, out Guid accountId)
    {
        var claim = user.FindFirst("accountId")?.Value;
        return Guid.TryParse(claim, out accountId);
    }
}
