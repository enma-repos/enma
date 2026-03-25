using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.ProjectMembers;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/members")]
[ApiController]
public sealed class ProjectMembersController(IProjectMembersService projectMembersService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromBody] AddProjectMemberDto dto,
        CancellationToken ct)
    {
        var res = await projectMembersService.AddAsync(organizationId, dto with { ProjectId = projectId }, ct);
        return res.ToActionResult();
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var res = await projectMembersService.GetAsync(projectId, userId, ct);
        return res.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await projectMembersService.ListByProjectAsync(projectId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{userId:guid}/role")]
    public async Task<IActionResult> SetRoleAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId,
        [FromBody] SetProjectMemberRoleDto dto,
        CancellationToken ct)
    {
        var res = await projectMembersService.SetRoleAsync(projectId, userId, dto, ct);
        return res.ToActionResult();
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> RemoveAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var res = await projectMembersService.RemoveAsync(projectId, userId, ct);
        return res.ToActionResult();
    }
}
