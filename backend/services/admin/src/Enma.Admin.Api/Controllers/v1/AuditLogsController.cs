using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.AuditLogs;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations/{organizationId:guid}")]
[ApiController]
[Authorize]
public sealed class AuditLogsController(IAuditLogsService auditLogsService) : ControllerBase
{
    [HttpPost("audit-logs")]
    public async Task<IActionResult> AppendAsync(
        [FromRoute] Guid organizationId,
        [FromBody] CreateAuditLogDto dto,
        CancellationToken ct)
    {
        if (!User.TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await auditLogsService.AppendAsync(
            dto with { OrganizationId = organizationId, ActorUserId = accountId }, ct);
        return res.ToActionResult();
    }

    [HttpGet("audit-logs")]
    public async Task<IActionResult> ListByOrgAsync(
        [FromRoute] Guid organizationId,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await auditLogsService.ListByOrgAsync(organizationId, from, to, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpGet("projects/{projectId:guid}/audit-logs")]
    public async Task<IActionResult> ListByProjectAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await auditLogsService.ListByProjectAsync(projectId, organizationId, from, to, offset, limit, ct);
        return res.ToActionResult();
    }
}
