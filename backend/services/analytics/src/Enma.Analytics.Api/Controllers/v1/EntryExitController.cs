using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.ValueObjects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Analytics.Api.Controllers.v1;

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/process-definitions/{processDefinitionId:guid}")]
[ApiController]
public sealed class EntryExitController(IEntryExitPointsService service) : ControllerBase
{
    [HttpGet("entry-exit")]
    public async Task<IActionResult> GetEntryExitPointsAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid processDefinitionId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int limit = 10,
        CancellationToken ct = default)
    {
        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        var filter = new ProcessFilter(organizationId, projectId, processDefinitionId, dateRange.Value);
        var res = await service.GetEntryExitPointsAsync(filter, limit, ct);
        return res.ToActionResult();
    }
}
