using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Projects;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class ProjectsService : IProjectsService
{
    private readonly IProjectsRepository _projectsRepository;

    public ProjectsService(IProjectsRepository projectsRepository)
    {
        _projectsRepository = projectsRepository;
    }

    public async Task<Result<ProjectDto>> CreateAsync(
        CreateProjectDto dto,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        
        var modelRes = Project.Create(
            id: Guid.NewGuid(),
            orgId: dto.OrganizationId,
            name: dto.Name,
            key: dto.Key,
            description: dto.Description,
            isStared: dto.IsStared,
            createdByUserId: dto.CreatedByUserId,
            settings: dto.Settings,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<ProjectDto>(modelRes.Errors);
        }

        var res = await _projectsRepository.CreateAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProjectDto>(res.Errors);
    }

    public async Task<Result<ProjectDto>> GetByIdAsync(
        Guid projectId, 
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.GetByIdAsync(projectId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProjectDto>(res.Errors);
    }

    public async Task<Result<ProjectDto>> GetByOrgAndKeyAsync(
        Guid orgId, 
        string key, 
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.GetByOrgAndKeyAsync(orgId, key, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProjectDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<ProjectDto>>> ListByOrgAsync(
        Guid orgId, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.ListByOrgAsync(orgId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<ProjectDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<ProjectDto>>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<ProjectDto>>> ListByUserAsync(
        Guid userId, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.ListByUserAsync(userId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<ProjectDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<ProjectDto>>(res.Errors);
    }

    public async Task<Result> SetNameAsync(
        Guid projectId, 
        SetProjectNameDto dto, 
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.SetNameAsync(projectId, dto.Name, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetDescriptionAsync(
        Guid projectId, 
        SetProjectDescriptionDto dto, 
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.SetDescriptionAsync(projectId, dto.Description, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetSettingsAsync(
        Guid projectId, 
        SetProjectSettingsDto dto, 
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.SetSettingsAsync(projectId, dto.Settings, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetArchivedAsync(
        Guid projectId, 
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.SetArchivedAsync(projectId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> ClearArchivedAsync(
        Guid projectId,
        CancellationToken ct = default)
    {
        var res = await _projectsRepository.ClearArchivedAsync(projectId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SoftDeleteAsync(
        Guid orgId,
        Guid projectId,
        CancellationToken ct = default)
    {
        var countRes = await _projectsRepository.CountByOrgAsync(orgId, ct);
        if (countRes.IsSuccess && countRes.Value <= 1)
        {
            return Result.Fail("Cannot delete the last project in the organization.");
        }

        var res = await _projectsRepository.SoftDeleteAsync(projectId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }
}
