using Enma.Admin.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Enma.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1.Super;

// Future: DELETE /{userId}, PATCH /{userId}, POST /{userId}/restore — mutation surface.
[Route("api/admin/v1/super/users")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
public sealed class SuperUsersController(ISuperUsersService superUsersService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> ListAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] bool includeDeleted = false,
        CancellationToken ct = default)
    {
        var res = await superUsersService.ListAsync(page, pageSize, search, includeDeleted, ct);
        return res.ToActionResult();
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid userId,
        CancellationToken ct)
    {
        var res = await superUsersService.GetByIdAsync(userId, ct);
        return res.ToActionResult();
    }
}
