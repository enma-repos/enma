using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.ValueObjects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Analytics.Api.Controllers.v1;

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/process-definitions/{processDefinitionId:guid}")]
[ApiController]
public sealed class FlowGraphController(IFlowGraphService service) : ControllerBase
{
    [HttpGet("flow")]
    public async Task<IActionResult> GetFlowAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid processDefinitionId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
    {
        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        var filter = new ProcessFilter(organizationId, projectId, processDefinitionId, dateRange.Value);
        var res = await service.GetFlowGraphAsync(filter, ct);
        return res.ToActionResult();
    }
}
