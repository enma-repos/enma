using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Analytics.Api.Controllers.v1;

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/process-definitions/{processDefinitionId:guid}")]
[ApiController]
[Authorize]
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

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}")]
[ApiController]
[Authorize]
public sealed class ProjectTopEventsController(ITopEventsService service) : ControllerBase
{
    [HttpPost("top-events")]
    public async Task<IActionResult> GetProjectTopEventsAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] string sortBy = "visits",
        [FromQuery] int limit = 20,
        [FromBody] TopEventsRequest? request = null,
        CancellationToken ct = default)
    {
        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        var res = await service.GetTopEventsAsync(
            organizationId, projectId,
            request?.ProcessDefinitionIds,
            dateRange.Value, sortBy, limit, ct);

        return res.ToActionResult();
    }
}
