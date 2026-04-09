using Enma.Admin.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Enma.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1.Super;

// Future: DELETE /{organizationId}, POST /{organizationId}/restore, PATCH /{organizationId}/owner.
[Route("api/admin/v1/super/organizations")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
public sealed class SuperOrganizationsController(ISuperOrganizationsService superOrganizationsService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> ListAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] bool includeDeleted = false,
        CancellationToken ct = default)
    {
        var res = await superOrganizationsService.ListAsync(page, pageSize, search, includeDeleted, ct);
        return res.ToActionResult();
    }

    [HttpGet("{organizationId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid organizationId,
        CancellationToken ct)
    {
        var res = await superOrganizationsService.GetByIdAsync(organizationId, ct);
        return res.ToActionResult();
    }
}
