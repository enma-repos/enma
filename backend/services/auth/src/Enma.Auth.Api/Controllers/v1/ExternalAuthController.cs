using Enma.Auth.Api.Dto;
using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Dto.Auth;
using Enma.Api.Shared.Extensions;
using Enma.Auth.Application.Options;
using Enma.Common.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Api.Controllers.v1;

[Route("/api/auth/v1/external")]
[ApiController]
public sealed class ExternalAuthController(
    IAuthService authService, 
    IOptions<JwtOptions> jwtOptions,
    IOptions<AuthOptions> authOptions,
    IWebHostEnvironment env) : ControllerBase
{
    [HttpPost("google/start")]
    public async Task<IActionResult> StartGoogleAuthAsync(
        [FromBody] StartExternalAuthRequestDto dto)
    {
        var result = await authService.GetProviderUrlAsync(dto with { Provider = "google" });
        return result.IsFailed
            ? result.ToActionResult()
            : Ok(new ExternalAuthStartResponseDto(result.Value));
    }
    
    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleAuthCallbackAsync(
        [FromQuery] string code,
        [FromQuery] string state,
        CancellationToken ct)
    {
        var result = await authService.AuthenticateExternalAsync(new ExternalAuthCallbackDto(
            Code: code,
            State: state), ct);

        if (result.IsFailed)
        {
            return result.ToActionResult();
        }

        var secure = !env.IsDevelopment();
        
        Response.Cookies.Append("access_token", result.Value.AuthTokens.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            MaxAge = TimeSpan.FromMinutes(jwtOptions.Value.ExpiresMinutes),
            Expires = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(jwtOptions.Value.ExpiresMinutes)
        });
        
        Response.Cookies.Append("refresh_token", result.Value.AuthTokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth",
            MaxAge = TimeSpan.FromDays(authOptions.Value.RefreshTokenLifetimeDays),
            Expires = DateTimeOffset.UtcNow + TimeSpan.FromDays(authOptions.Value.RefreshTokenLifetimeDays)
        });
        
        var baseUri = new Uri(authOptions.Value.RedirectBaseAddress, UriKind.Absolute);
        if (!Uri.TryCreate(result.Value.SuccessUrl, UriKind.Relative, out var rel))
        {
            return BadRequest("Invalid successUrl");
        }
        
        var redirectUri = new Uri(baseUri, rel);
        return Redirect(redirectUri.ToString());
    }
}
