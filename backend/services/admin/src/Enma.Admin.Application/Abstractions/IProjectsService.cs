using Enma.Admin.Application.Dto.Projects;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IProjectsService
{
    Task<Result<ProjectDto>> CreateAsync(CreateProjectDto dto, CancellationToken ct = default);
    Task<Result<ProjectDto>> GetByIdAsync(Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result<ProjectDto>> GetByOrgAndKeyAsync(Guid orgId, string key, CancellationToken ct = default);

    Task<Result<IReadOnlyList<ProjectDto>>> ListByOrgAsync(
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result<IReadOnlyList<ProjectDto>>> ListByUserAsync(
        Guid userId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetNameAsync(Guid projectId, Guid orgId, SetProjectNameDto dto, CancellationToken ct = default);
    Task<Result> SetDescriptionAsync(Guid projectId, Guid orgId, SetProjectDescriptionDto dto, CancellationToken ct = default);
    Task<Result> SetSettingsAsync(Guid projectId, Guid orgId, SetProjectSettingsDto dto, CancellationToken ct = default);
    Task<Result> SetArchivedAsync(Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result> ClearArchivedAsync(Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid orgId, Guid projectId, CancellationToken ct = default);
}
