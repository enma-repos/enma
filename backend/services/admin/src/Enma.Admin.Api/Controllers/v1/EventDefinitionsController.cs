using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.EventDefinitions;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/event-definitions")]
[ApiController]
public sealed class EventDefinitionsController(IEventDefinitionsService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromBody] CreateEventDefinitionDto dto,
        CancellationToken ct)
    {
        var res = await service.CreateAsync(dto with { ProjectId = projectId }, ct);
        return res.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var res = await service.GetByIdAsync(id, ct);
        return res.ToActionResult();
    }

    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetByProjectAndNameAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] string name,
        CancellationToken ct)
    {
        var res = await service.GetByProjectAndNameAsync(projectId, name, ct);
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
        var res = await service.ListByProjectAsync(projectId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{id:guid}/description")]
    public async Task<IActionResult> SetDescriptionAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid id,
        [FromBody] SetEventDefinitionDescriptionDto dto,
        CancellationToken ct)
    {
        var res = await service.SetDescriptionAsync(id, dto, ct);
        return res.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var res = await service.SoftDeleteAsync(id, ct);
        return res.ToActionResult();
    }
}
