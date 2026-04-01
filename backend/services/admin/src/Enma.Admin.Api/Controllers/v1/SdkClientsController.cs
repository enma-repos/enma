using Enma.Admin.Api.Filters;
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
    [AuditAction("create", "SdkClient")]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromBody] CreateSdkClientDto dto,
        CancellationToken ct)
    {
        var res = await sdkClientsService.CreateAsync(dto with { ProjectId = projectId }, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        CancellationToken ct)
    {
        var res = await sdkClientsService.GetByIdAsync(clientId, projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> ListByProjectAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var res = await sdkClientsService.ListByProjectAsync(projectId, organizationId, page, pageSize, search, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/name")]
    [AuditAction("update.name", "SdkClient", ResourceIdParam = "clientId")]
    public async Task<IActionResult> SetNameAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        [FromBody] SetSdkClientNameDto dto,
        CancellationToken ct)
    {
        var res = await sdkClientsService.SetNameAsync(clientId, projectId, organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/settings")]
    [AuditAction("update.settings", "SdkClient", ResourceIdParam = "clientId")]
    public async Task<IActionResult> SetSettingsAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        [FromBody] SetSdkClientSettingsDto dto,
        CancellationToken ct)
    {
        var res = await sdkClientsService.SetSettingsAsync(clientId, projectId, organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/type")]
    [AuditAction("update.type", "SdkClient", ResourceIdParam = "clientId")]
    public async Task<IActionResult> SetTypeAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        [FromBody] SetSdkClientTypeDto dto,
        CancellationToken ct)
    {
        var res = await sdkClientsService.SetTypeAsync(clientId, projectId, organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/disable")]
    [AuditAction("disable", "SdkClient", ResourceIdParam = "clientId")]
    public async Task<IActionResult> DisableAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        CancellationToken ct)
    {
        var res = await sdkClientsService.SetDisabledAsync(clientId, projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{clientId:guid}/enable")]
    [AuditAction("enable", "SdkClient", ResourceIdParam = "clientId")]
    public async Task<IActionResult> EnableAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid clientId,
        CancellationToken ct)
    {
        var res = await sdkClientsService.ClearDisabledAsync(clientId, projectId, organizationId, ct);
        return res.ToActionResult();
    }
}
