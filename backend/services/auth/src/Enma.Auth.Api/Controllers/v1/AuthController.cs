using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Dto.Auth;
using Enma.Api.Shared.Extensions;
using Enma.Common.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Api.Controllers.v1;

[Route("/api/auth/v1")]
[ApiController]
public sealed class AuthController(
    IAuthService authService,
    IOptions<JwtOptions> jwtOptions,
    IWebHostEnvironment env) : ControllerBase
{
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(CancellationToken ct)
    {
        var refreshToken = Request.Cookies["refresh_token"];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized();
        }

        var result = await authService.RefreshAsync(new RefreshTokensDto(refreshToken), ct);
        if (result.IsFailed)
        {
            return result.ToActionResult();
        }

        var now = DateTimeOffset.UtcNow;
        var accessExpiresAt = now + TimeSpan.FromMinutes(jwtOptions.Value.ExpiresMinutes);
        var refreshExpiresAt = new DateTimeOffset(result.Value.RefreshTokenExpiresAt, TimeSpan.Zero);
        var secure = !env.IsDevelopment();

        Response.Cookies.Append("access_token", result.Value.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            MaxAge = accessExpiresAt - now,
            Expires = accessExpiresAt
        });

        Response.Cookies.Append("refresh_token", result.Value.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth",
            MaxAge = refreshExpiresAt - now,
            Expires = refreshExpiresAt
        });

        return Ok(result.Value);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(
        [FromBody] LogoutDto dto,
        CancellationToken ct)
    {
        var result = await authService.LogoutAsync(dto, ct);
        if (result.IsSuccess)
        {
            Response.Cookies.Delete("access_token", new CookieOptions { Path = "/" });
            Response.Cookies.Delete("refresh_token", new CookieOptions { Path = "/api/auth" });
        }
        
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
