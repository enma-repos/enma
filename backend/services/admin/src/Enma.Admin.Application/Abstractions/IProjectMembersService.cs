using Enma.Admin.Application.Dto.ProjectMembers;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IProjectMembersService
{
    Task<Result<ProjectMemberDto>> AddAsync(AddProjectMemberDto dto, CancellationToken ct = default);
    Task<Result<ProjectMemberDto>> GetAsync(Guid projectId, Guid userId, CancellationToken ct = default);

    Task<Result<IReadOnlyList<ProjectMemberDto>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetRoleAsync(
        Guid projectId,
        Guid userId,
        SetProjectMemberRoleDto dto,
        CancellationToken ct = default);

    Task<Result> RemoveAsync(Guid projectId, Guid userId, CancellationToken ct = default);
}
