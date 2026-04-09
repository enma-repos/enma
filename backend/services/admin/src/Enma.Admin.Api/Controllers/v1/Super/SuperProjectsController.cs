using Enma.Admin.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Enma.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1.Super;

// Future: DELETE /{projectId}, POST /{projectId}/restore, POST /{projectId}/archive, POST /{projectId}/unarchive.
[Route("api/admin/v1/super/projects")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
public sealed class SuperProjectsController(ISuperProjectsService superProjectsService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> ListAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] bool includeDeleted = false,
        [FromQuery] Guid? organizationId = null,
        CancellationToken ct = default)
    {
        var res = await superProjectsService.ListAsync(page, pageSize, search, includeDeleted, organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet("{projectId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid projectId,
        CancellationToken ct)
    {
        var res = await superProjectsService.GetByIdAsync(projectId, ct);
        return res.ToActionResult();
    }
}
