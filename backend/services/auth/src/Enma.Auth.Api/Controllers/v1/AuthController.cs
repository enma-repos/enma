using Microsoft.AspNetCore.Mvc;

namespace Enma.Auth.Api.Controllers.v1;

[Route("/api/auth/v1")]
[ApiController]
public sealed class AuthController : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}