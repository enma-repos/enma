using Enma.Admin.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/sdk-clients/{clientId:guid}/api-keys")]
[ApiController]
[Authorize]
public sealed class ApiKeysController(IApiKeysService apiKeysService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid clientId, 
        [FromRoute] Guid organizationId, 
        [FromRoute] Guid projectId, 
        CancellationToken ct)
    {
        var res = await apiKeysService.CreateAsync(clientId, projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet("{apiKeyId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid apiKeyId,
        [FromRoute] Guid clientId,
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        CancellationToken ct)
    {
        var res = await apiKeysService.GetByIdAsync(apiKeyId, clientId, projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> ListBySdkClientAsync(
        [FromRoute] Guid clientId,
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await apiKeysService.ListBySdkClientAsync(clientId, projectId, organizationId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpGet("~/api/v1/api-keys/active")]
    public async Task<IActionResult> ListActiveByPrefixAsync(
        [FromRoute] Guid clientId, 
        [FromRoute] Guid organizationId, 
        [FromRoute] Guid projectId, 
        [FromQuery] string keyPrefix, 
        [FromQuery] int limit = 50, 
        CancellationToken ct = default)
    {
        var res = await apiKeysService.ListActiveByPrefixAsync(keyPrefix, limit, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{apiKeyId:guid}/last-used")]
    public async Task<IActionResult> UpdateLastUsedAsync(
        [FromRoute] Guid apiKeyId, 
        [FromRoute] Guid clientId, 
        [FromRoute] Guid organizationId, 
        [FromRoute] Guid projectId, 
        CancellationToken ct)
    {
        var res = await apiKeysService.UpdateLastUsedAsync(apiKeyId, clientId, projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{apiKeyId:guid}/revoke")]
    public async Task<IActionResult> RevokeAsync(
        [FromRoute] Guid apiKeyId,
        [FromRoute] Guid clientId,
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        CancellationToken ct)
    {
        var res = await apiKeysService.UpdateRevokedAsync(apiKeyId, clientId, projectId, organizationId, ct);
        return res.ToActionResult();
    }
}
