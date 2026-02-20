using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.ProjectMembers;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class ProjectMembersService : IProjectMembersService
{
    private readonly IProjectMembersRepository _projectMembersRepository;

    public ProjectMembersService(IProjectMembersRepository projectMembersRepository)
    {
        _projectMembersRepository = projectMembersRepository;
    }

    public async Task<Result<ProjectMemberDto>> AddAsync(
        AddProjectMemberDto dto, 
        CancellationToken ct = default)
    {
        // TODO: check if user is a member of organization
        
        var now = DateTime.UtcNow;

        var modelRes = ProjectMember.Create(
            projectId: dto.ProjectId,
            userId: dto.UserId,
            role: dto.Role,
            joinedAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<ProjectMemberDto>(modelRes.Errors);
        }

        var res = await _projectMembersRepository.AddAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProjectMemberDto>(res.Errors);
    }

    public async Task<Result<ProjectMemberDto>> GetAsync(
        Guid projectId, 
        Guid userId, 
        CancellationToken ct = default)
    {
        var res = await _projectMembersRepository.GetAsync(projectId, userId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProjectMemberDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<ProjectMemberDto>>> ListByProjectAsync(
        Guid projectId, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _projectMembersRepository.ListByProjectAsync(projectId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<ProjectMemberDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<ProjectMemberDto>>(res.Errors);
    }

    public async Task<Result> SetRoleAsync(
        Guid projectId, 
        Guid userId, 
        SetProjectMemberRoleDto dto, 
        CancellationToken ct = default)
    {
        var res = await _projectMembersRepository.SetRoleAsync(projectId, userId, dto.Role, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public Task<Result> RemoveAsync(Guid projectId, Guid userId, CancellationToken ct = default)
        => _projectMembersRepository.RemoveAsync(projectId, userId, ct);
}
