using Enma.Admin.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/v1/organizations/{organizationId}/projects/{projectId}/sdk-clients/{clientId}")]
[ApiController]
public sealed class ApiKeysController(IApiKeysService apiKeysService) : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}