using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.OrganizationInvites;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}/invites")]
[ApiController]
[Authorize]
public sealed class OrganizationInvitesController(IOrganizationInvitesService organizationInvitesService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid organizationId,
        [FromBody] CreateOrganizationInviteDto dto,
        CancellationToken ct)
    {
        if (!User.TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await organizationInvitesService.CreateAsync(
            dto with { OrganizationId = organizationId, CreatedByUserId = accountId }, ct);
        return res.ToActionResult();
    }

    [HttpGet("{inviteId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid inviteId,
        CancellationToken ct)
    {
        var res = await organizationInvitesService.GetByIdAsync(inviteId, ct);
        return res.ToActionResult();
    }

    [HttpGet("active")]
    public async Task<IActionResult> ListActiveAsync(
        [FromRoute] Guid organizationId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await organizationInvitesService.ListActiveByOrgAsync(organizationId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpGet("active/by-email")]
    public async Task<IActionResult> GetActiveByEmailAsync(
        [FromRoute] Guid organizationId,
        [FromQuery] string email,
        CancellationToken ct)
    {
        var res = await organizationInvitesService.GetActiveByOrgAndEmailAsync(organizationId, email, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{inviteId:guid}/accept")]
    public async Task<IActionResult> AcceptAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid inviteId,
        CancellationToken ct)
    {
        if (!User.TryGetAccountId(out var accountId))
            return Unauthorized();

        var dto = new SetInviteAcceptedDto(accountId);
        var res = await organizationInvitesService.SetAcceptedAsync(inviteId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{inviteId:guid}/decline")]
    public async Task<IActionResult> DeclineAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid inviteId,
        CancellationToken ct)
    {
        if (!User.TryGetAccountId(out var accountId))
            return Unauthorized();

        var dto = new SetInviteDeclinedDto(accountId);
        var res = await organizationInvitesService.SetDeclinedAsync(inviteId, dto, ct);
        return res.ToActionResult();
    }
}
