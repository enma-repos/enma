using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.Organizations;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/admin/v1/organizations")]
[ApiController]
public sealed class OrganizationsController(IOrganizationsService organizationsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateOrganizationDto dto,
        CancellationToken ct)
    {
        var res = await organizationsService.CreateAsync(dto, ct);
        return res.ToActionResult();
    }

    [HttpGet("{organizationId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid organizationId,
        CancellationToken ct)
    {
        var res = await organizationsService.GetByIdAsync(organizationId, ct);
        return res.ToActionResult();
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<IActionResult> GetBySlugAsync(
        [FromRoute] string slug,
        CancellationToken ct)
    {
        var res = await organizationsService.GetBySlugAsync(slug, ct);
        return res.ToActionResult();
    }

    [HttpGet("~/api/v1/users/{userId:guid}/organizations")]
    public async Task<IActionResult> ListByUserAsync(
        [FromRoute] Guid userId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        CancellationToken ct = default)
    {
        var res = await organizationsService.ListByUserAsync(userId, offset, limit, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{organizationId:guid}/name")]
    public async Task<IActionResult> SetNameAsync(
        [FromRoute] Guid organizationId,
        [FromBody] SetOrganizationNameDto dto,
        CancellationToken ct)
    {
        var res = await organizationsService.SetNameAsync(organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpPatch("{organizationId:guid}/owner")]
    public async Task<IActionResult> SetOwnerAsync(
        [FromRoute] Guid organizationId, 
        [FromBody] SetOrganizationOwnerDto dto,
        CancellationToken ct)
    {
        var res = await organizationsService.SetOwnerAsync(organizationId, dto, ct);
        return res.ToActionResult();
    }

    [HttpDelete("{organizationId:guid}")]
    public async Task<IActionResult> SoftDeleteAsync(
        [FromRoute] Guid organizationId,
        CancellationToken ct)
    {
        var res = await organizationsService.SoftDeleteAsync(organizationId, ct);
        return res.ToActionResult();
    }
}
