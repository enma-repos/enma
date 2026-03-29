using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Analytics.Api.Controllers.v1;

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}")]
[ApiController]
[Authorize]
public sealed class SummaryController(ISummaryService service) : ControllerBase
{
    [HttpPost("summary")]
    public async Task<IActionResult> GetSummaryAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromBody] SummaryRequest request,
        CancellationToken ct)
    {
        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        var res = await service.GetSummaryAsync(
            organizationId, projectId,
            request.ProcessDefinitionIds,
            dateRange.Value, ct);

        return res.ToActionResult();
    }
}
