using Enma.Admin.Application.Dto.Projects;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IProjectsService
{
    Task<Result<ProjectDto>> CreateAsync(CreateProjectDto dto, CancellationToken ct = default);
    Task<Result<ProjectDto>> GetByIdAsync(Guid projectId, CancellationToken ct = default);
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

    Task<Result> SetNameAsync(Guid projectId, SetProjectNameDto dto, CancellationToken ct = default);
    Task<Result> SetDescriptionAsync(Guid projectId, SetProjectDescriptionDto dto, CancellationToken ct = default);
    Task<Result> SetSettingsAsync(Guid projectId, SetProjectSettingsDto dto, CancellationToken ct = default);
    Task<Result> SetArchivedAsync(Guid projectId, CancellationToken ct = default);
    Task<Result> ClearArchivedAsync(Guid projectId, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid orgId, Guid projectId, CancellationToken ct = default);
}
