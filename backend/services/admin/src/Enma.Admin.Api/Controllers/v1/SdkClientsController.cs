using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.SdkClients;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/sdk-clients")]
[ApiController]
[Authorize]
public sealed class SdkClientsController(ISdkClientsService sdkClientsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromBody] CreateSdkClientDto dto,
        CancellationToken ct)
    {
        var res = await sdkClientsService.CreateAsync(dto with { ProjectId = projectId }, ct);
        return res.ToActionResult();
    }

    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        CancellationToken ct)
    {
        var res = await sdkClientsService.GetByIdAsync(clientId, ct);
        return res.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> ListByProjectAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await sdkClientsService.ListByProjectAsync(projectId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/name")]
    public async Task<IActionResult> SetNameAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        [FromBody] SetSdkClientNameDto dto,
        CancellationToken ct)
    {
        var res = await sdkClientsService.SetNameAsync(clientId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/settings")]
    public async Task<IActionResult> SetSettingsAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId, 
        [FromBody] SetSdkClientSettingsDto dto,
        CancellationToken ct)
    {
        var res = await sdkClientsService.SetSettingsAsync(clientId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/type")]
    public async Task<IActionResult> SetTypeAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId, 
        [FromBody] SetSdkClientTypeDto dto,
        CancellationToken ct)
    {
        var res = await sdkClientsService.SetTypeAsync(clientId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/disable")]
    public async Task<IActionResult> DisableAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId, 
        CancellationToken ct)
    {
        var res = await sdkClientsService.SetDisabledAsync(clientId, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/enable")]
    public async Task<IActionResult> EnableAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        CancellationToken ct)
    {
        var res = await sdkClientsService.ClearDisabledAsync(clientId, ct);
        return res.ToActionResult();
    }
}
