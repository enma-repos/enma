using System.Security.Claims;

namespace Enma.Common.Constants;

public static class AuthorizationPolicies
{
    public const string SuperAdmin = "SuperAdmin";

    // Use the long URI form — JwtBearer's DefaultInboundClaimTypeMap remaps the
    // short "role" claim from the JWT body to ClaimTypes.Role on the ClaimsPrincipal,
    // so reading code (policies, extensions) must use the long form to find it.
    public const string RoleClaimType = ClaimTypes.Role;
}
