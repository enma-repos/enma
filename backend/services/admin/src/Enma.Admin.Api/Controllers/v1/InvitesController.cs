using Enma.Admin.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/invites")]
[ApiController]
[Authorize]
public sealed class InvitesController(IOrganizationInvitesService organizationInvitesService) : ControllerBase
{
    [HttpGet("pending")]
    public async Task<IActionResult> ListPendingAsync(
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 20,
        CancellationToken ct = default)
    {
        if (!User.TryGetAccountId(out var accountId))
            return Unauthorized();

        var res = await organizationInvitesService.ListPendingByEmailAsync(accountId, offset, limit, ct);
        return res.ToActionResult();
    }
}
