using Enma.Admin.Application.Dto.Organizations;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IOrganizationsService
{
    Task<Result<OrganizationDto>> CreateAsync(CreateOrganizationDto dto, CancellationToken ct = default);
    Task<Result<OrganizationDto>> GetByIdAsync(Guid orgId, CancellationToken ct = default);
    Task<Result<OrganizationDto>> GetBySlugAsync(string slug, CancellationToken ct = default);

    Task<Result<IReadOnlyList<OrganizationDto>>> ListByUserAsync(
        Guid userId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetNameAsync(Guid orgId, SetOrganizationNameDto dto, CancellationToken ct = default);
    Task<Result> SetOwnerAsync(Guid orgId, SetOrganizationOwnerDto dto, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid orgId, CancellationToken ct = default);
}
