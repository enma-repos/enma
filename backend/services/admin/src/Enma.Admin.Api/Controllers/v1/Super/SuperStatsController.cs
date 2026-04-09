using Enma.Admin.Application.Abstractions;
using Enma.Api.Shared.Extensions;
using Enma.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1.Super;

[Route("api/admin/v1/super/stats")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
public sealed class SuperStatsController(ISuperStatsService superStatsService) : ControllerBase
{
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverviewAsync(CancellationToken ct)
    {
        var res = await superStatsService.GetOverviewAsync(ct);
        return res.ToActionResult();
    }
}
