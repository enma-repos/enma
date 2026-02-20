using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.OrganizationMembers;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}/members")]
[ApiController]
public sealed class OrganizationMembersController(IOrganizationMembersService organizationMembersService) 
    : ControllerBase
{
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var res = await organizationMembersService.GetAsync(organizationId, userId, ct);
        return res.ToActionResult();
    }

    [HttpGet("{userId:guid}/is-member")]
    public async Task<IActionResult> IsMemberAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var res = await organizationMembersService.IsMemberAsync(organizationId, userId, ct);
        return res.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync(
        [FromRoute] Guid organizationId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await organizationMembersService.ListByOrgAsync(organizationId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{userId:guid}/role")]
    public async Task<IActionResult> SetRoleAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid userId,
        [FromBody] SetOrganizationMemberRoleDto dto,
        CancellationToken ct)
    {
        var res = await organizationMembersService.SetRoleAsync(organizationId, userId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{userId:guid}/status")]
    public async Task<IActionResult> SetStatusAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid userId,
        [FromBody] SetOrganizationMemberStatusDto dto,
        CancellationToken ct)
    {
        var res = await organizationMembersService.SetStatusAsync(organizationId, userId, dto, ct);
        return res.ToActionResult();
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> RemoveAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var res = await organizationMembersService.RemoveAsync(organizationId, userId, ct);
        return res.ToActionResult();
    }
}
