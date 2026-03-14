using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.ValueObjects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Analytics.Api.Controllers.v1;

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/process-definitions/{processDefinitionId:guid}")]
[ApiController]
public sealed class TopEventsController(ITopEventsService service) : ControllerBase
{
    [HttpGet("top-events")]
    public async Task<IActionResult> GetTopEventsAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid processDefinitionId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] string sortBy = "visits",
        [FromQuery] int limit = 20,
        CancellationToken ct = default)
    {
        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        var filter = new ProcessFilter(organizationId, projectId, processDefinitionId, dateRange.Value);
        var res = await service.GetTopEventsAsync(filter, sortBy, limit, ct);
        return res.ToActionResult();
    }
}
