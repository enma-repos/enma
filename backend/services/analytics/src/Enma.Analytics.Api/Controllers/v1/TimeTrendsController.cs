using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Enums;
using Enma.Analytics.Application.ValueObjects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Analytics.Api.Controllers.v1;

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/process-definitions/{processDefinitionId:guid}")]
[ApiController]
public sealed class TimeTrendsController(ITimeTrendsService service) : ControllerBase
{
    [HttpGet("trends")]
    public async Task<IActionResult> GetTimeTrendsAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid processDefinitionId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] string granularity = "Hour",
        CancellationToken ct = default)
    {
        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        if (!Enum.TryParse<Granularity>(granularity, ignoreCase: true, out var gran))
            return BadRequest(new { error = "Invalid granularity. Allowed values: FiveMinutes, Hour, Day." });

        var filter = new ProcessFilter(organizationId, projectId, processDefinitionId, dateRange.Value);
        var res = await service.GetTimeTrendsAsync(filter, gran, ct);
        return res.ToActionResult();
    }
}
