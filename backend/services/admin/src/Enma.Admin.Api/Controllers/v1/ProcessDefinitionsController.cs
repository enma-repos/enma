using Enma.Admin.Api.Filters;
using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.ProcessDefinitions;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/process-definitions")]
[ApiController]
[Authorize]
public sealed class ProcessDefinitionsController(IProcessDefinitionsService service) : ControllerBase
{
    [HttpPost]
    [AuditAction("create", "ProcessDefinition")]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromBody] CreateProcessDefinitionDto dto,
        CancellationToken ct)
    {
        if (!User.TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await service.CreateAsync(
            dto with { ProjectId = projectId, CreatedByUserId = accountId }, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var res = await service.GetByIdAsync(id, projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet("by-key/{key}")]
    public async Task<IActionResult> GetByProjectAndKeyAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] string key,
        CancellationToken ct)
    {
        var res = await service.GetByProjectAndKeyAsync(projectId, organizationId, key, ct);
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
        var res = await service.ListByProjectAsync(projectId, organizationId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{id:guid}/name")]
    [AuditAction("update.name", "ProcessDefinition", ResourceIdParam = "id")]
    public async Task<IActionResult> SetNameAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid id,
        [FromBody] SetProcessDefinitionNameDto dto,
        CancellationToken ct)
    {
        var res = await service.SetNameAsync(id, projectId, organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{id:guid}/description")]
    [AuditAction("update.description", "ProcessDefinition", ResourceIdParam = "id")]
    public async Task<IActionResult> SetDescriptionAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid id,
        [FromBody] SetProcessDefinitionDescriptionDto dto,
        CancellationToken ct)
    {
        var res = await service.SetDescriptionAsync(id, projectId, organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    [AuditAction("delete", "ProcessDefinition", ResourceIdParam = "id")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var res = await service.SoftDeleteAsync(id, projectId, organizationId, ct);
        return res.ToActionResult();
    }
}
