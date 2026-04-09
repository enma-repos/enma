using Enma.Admin.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Enma.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1.Super;

[Route("api/admin/v1/super/audit-logs")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
public sealed class SuperAuditLogsController(ISuperAuditLogsService superAuditLogsService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> ListAsync(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] string? action = null,
        [FromQuery] string? resourceType = null,
        [FromQuery] Guid? actorUserId = null,
        [FromQuery] Guid? organizationId = null,
        [FromQuery] Guid? projectId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var res = await superAuditLogsService.ListAsync(
            from, to, action, resourceType, actorUserId, organizationId, projectId,
            page, pageSize, search, ct);
        return res.ToActionResult();
    }
}
