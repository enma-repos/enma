using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.Projects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}/projects")]
[ApiController]
[Authorize]
public sealed class ProjectsController(IProjectsService projectsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid organizationId,
        [FromBody] CreateProjectDto dto,
        CancellationToken ct)
    {
        if (!User.TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await projectsService.CreateAsync(
            dto with { OrganizationId = organizationId, CreatedByUserId = accountId }, ct);
        return res.ToActionResult();
    }

    [HttpGet("{projectId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid organizationId, 
        [FromRoute] Guid projectId,
        CancellationToken ct)
    {
        var res = await projectsService.GetByIdAsync(projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet("by-key/{key}")]
    public async Task<IActionResult> GetByOrgAndKeyAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] string key,
        CancellationToken ct)
    {
        var res = await projectsService.GetByOrgAndKeyAsync(organizationId, key, ct);
        return res.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> ListByOrgAsync(
        [FromRoute] Guid organizationId,  
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await projectsService.ListByOrgAsync(organizationId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpGet("~/api/admin/v1/me/projects")]
    public async Task<IActionResult> ListByUserAsync(
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        if (!User.TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await projectsService.ListByUserAsync(accountId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{projectId:guid}/name")]
    public async Task<IActionResult> SetNameAsync(
        [FromRoute] Guid organizationId, 
        [FromRoute] Guid projectId, 
        [FromBody] SetProjectNameDto dto,
        CancellationToken ct)
    {
        var res = await projectsService.SetNameAsync(projectId, organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{projectId:guid}/description")]
    public async Task<IActionResult> SetDescriptionAsync(
        [FromRoute] Guid organizationId, 
        [FromRoute] Guid projectId,
        [FromBody] SetProjectDescriptionDto dto, 
        CancellationToken ct)
    {
        var res = await projectsService.SetDescriptionAsync(projectId, organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{projectId:guid}/settings")]
    public async Task<IActionResult> SetSettingsAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromBody] SetProjectSettingsDto dto,
        CancellationToken ct)
    {
        var res = await projectsService.SetSettingsAsync(projectId, organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{projectId:guid}/archive")]
    public async Task<IActionResult> ArchiveAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        CancellationToken ct)
    {
        var res = await projectsService.SetArchivedAsync(projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{projectId:guid}/unarchive")]
    public async Task<IActionResult> UnarchiveAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        CancellationToken ct)
    {
        var res = await projectsService.ClearArchivedAsync(projectId, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpDelete("{projectId:guid}")]
    public async Task<IActionResult> SoftDeleteAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        CancellationToken ct)
    {
        var res = await projectsService.SoftDeleteAsync(organizationId, projectId, ct);
        return res.ToActionResult();
    }
}
