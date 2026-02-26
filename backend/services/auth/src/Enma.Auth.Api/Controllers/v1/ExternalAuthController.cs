using Enma.Auth.Api.Dto;
using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Dto.Auth;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Auth.Api.Controllers.v1;

[Route("/api/auth/v1/external")]
[ApiController]
public sealed class ExternalAuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("google/start")]
    public async Task<IActionResult> StartGoogleAuthAsync(
        [FromBody] StartExternalAuthRequestDto dto)
    {
        var result = await authService.GetProviderUrlAsync(new StartExternalAuthRequestDto(
            Provider: "google",
            SuccessUrl: dto.SuccessUrl));

        return result.IsFailed
            ? result.ToActionResult()
            : Ok(new ExternalAuthStartResponseDto(result.Value));
    }
    
    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleAuthCallbackAsync(
        [FromQuery] string code,
        [FromQuery] string state,
        [FromQuery] string? scope,
        CancellationToken ct)
    {
        var result = await authService.AuthenticateExternalAsync(new ExternalAuthCallbackDto(
            Code: code,
            State: state), ct);

        if (result.IsFailed)
        {
            return result.ToActionResult();
        }

        return Ok(new ExternalAuthCallbackResponseDto(
            Tokens: result.Value.AuthTokens,
            SuccessUrl: result.Value.SuccessUrl));
    }
}
