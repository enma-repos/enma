using Enma.Auth.Application.Dto.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Auth.Api.Controllers.v1;

[Route("/api/auth/v1/external")]
[ApiController]
public sealed class ExternalAuthController : ControllerBase
{
    [HttpPost("google/start")]
    public async Task<IActionResult> StartGoogleAuthAsync(
        [FromBody] StartExternalAuthRequestDto dto,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleAuthCallbackAsync(
        [FromQuery] string code,
        [FromQuery] string state,
        [FromQuery] string? scope,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}