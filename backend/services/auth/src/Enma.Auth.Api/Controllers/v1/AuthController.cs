using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Dto.Auth;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Auth.Api.Controllers.v1;

[Route("/api/auth/v1")]
[ApiController]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(
        [FromBody] RefreshTokensDto dto,
        CancellationToken ct)
    {
        var result = await authService.RefreshAsync(dto, ct);
        return result.ToActionResult();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(
        [FromBody] LogoutDto dto,
        CancellationToken ct)
    {
        var result = await authService.LogoutAsync(dto, ct);
        return result.ToActionResult();
    }

	    [HttpGet("me")]
	    [Authorize]
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

	    [HttpPost("onboarding/complete")]
	    [Authorize]
	    public async Task<IActionResult> CompleteOnboardingAsync(
	        [FromBody] CompleteOnboardingDto dto,
	        CancellationToken ct)
	    {
        var accountIdClaim = User.FindFirst("accountId")?.Value;
        if (!Guid.TryParse(accountIdClaim, out var accountId))
        {
            return Unauthorized();
        }

        var result = await authService.CompleteOnboardingAsync(accountId, dto, ct);
        return result.ToActionResult();
    }
}
