using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Analytics.Api.Controllers.v1;

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/process-definitions/{processDefinitionId:guid}")]
[ApiController]
public sealed class FunnelController(IFunnelAnalysisService service) : ControllerBase
{
    [HttpPost("funnel")]
    public async Task<IActionResult> AnalyzeAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid processDefinitionId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromBody] FunnelAnalysisRequest request,
        CancellationToken ct)
    {
        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        var filter = new ProcessFilter(organizationId, projectId, processDefinitionId, dateRange.Value);
        var res = await service.AnalyzeAsync(filter, request, ct);
        return res.ToActionResult();
    }
}
