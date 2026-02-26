using Enma.Auth.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Auth.Api.Controllers.v1;

[Route("/api/auth/v1")]
[ApiController]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync(CancellationToken ct)
    {
        var accountIdClaim = User.FindFirst("accountId")?.Value;
        if (!Guid.TryParse(accountIdClaim, out var accountId))
        {
            return Unauthorized();
        }

        var res = await authService.GetMeAsync(accountId, ct);
        return res.ToActionResult();
    }
}
