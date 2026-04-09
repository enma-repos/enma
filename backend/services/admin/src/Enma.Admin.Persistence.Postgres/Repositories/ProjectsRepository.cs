using System.Text.Json.Nodes;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Super;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class ProjectsRepository : IProjectsRepository
{
    private readonly PostgresDbContext _context;

    public ProjectsRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Project>> CreateAsync(Project project, CancellationToken ct = default)
    {
        _context.Projects.Add(project.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(project);
    }

    public async Task<Result<Project>> GetByIdAsync(Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var entity = await _context.Projects
            .AsNoTracking()
            .Where(x => x.Id == projectId && x.OrganizationId == orgId && x.DeletedAt == null)
            .Select(x => new ProjectEntity
            {
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                Name = x.Name,
                Key = x.Key,
                Description = x.Description,
                IsStared = x.IsStared,
                CreatedByUserId = x.CreatedByUserId,
                Settings = x.Settings,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                ArchivedAt = x.ArchivedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<Project>(ApplicationErrors.EntityNotFound("Project", $"id={projectId}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<Project>> GetByOrgAndKeyAsync(Guid orgId, string key, CancellationToken ct = default)
    {
        var entity = await _context.Projects
            .AsNoTracking()
            .Where(x => x.OrganizationId == orgId && x.Key == key && x.DeletedAt == null)
            .Select(x => new ProjectEntity
            {
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                Name = x.Name,
                Key = x.Key,
                Description = x.Description,
                IsStared = x.IsStared,
                CreatedByUserId = x.CreatedByUserId,
                Settings = x.Settings,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                ArchivedAt = x.ArchivedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<Project>(ApplicationErrors.EntityNotFound("Project", $"orgId={orgId}, key={key}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<IReadOnlyList<Project>>> ListByOrgAsync(Guid orgId, int offset, int limit, CancellationToken ct = default)
    {
        var entities = await _context.Projects
            .AsNoTracking()
            .Where(x => x.OrganizationId == orgId && x.DeletedAt == null)
            .Skip(offset)
            .Take(limit)
            .Select(x => new ProjectEntity
            {
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                Name = x.Name,
                Key = x.Key,
                Description = x.Description,
                IsStared = x.IsStared,
                CreatedByUserId = x.CreatedByUserId,
                Settings = x.Settings,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                ArchivedAt = x.ArchivedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<Project>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<IReadOnlyList<Project>>> ListByUserAsync(Guid userId, int offset, int limit, CancellationToken ct = default)
    {
        var entities = await _context.Projects
            .AsNoTracking()
            .Where(p =>
                p.DeletedAt == null &&
                _context.ProjectMembers.Any(m => m.ProjectId == p.Id && m.UserId == userId))
            .Skip(offset)
            .Take(limit)
            .Select(x => new ProjectEntity
            {
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                Name = x.Name,
                Key = x.Key,
                Description = x.Description,
                IsStared = x.IsStared,
                CreatedByUserId = x.CreatedByUserId,
                Settings = x.Settings,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                ArchivedAt = x.ArchivedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<Project>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> UpdateAsync(Project project, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == project.Id && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Name, project.Name)
                    .SetProperty(x => x.Description, project.Description)
                    .SetProperty(x => x.IsStared, project.IsStared)
                    .SetProperty(x => x.Settings, project.Settings)
                    .SetProperty(x => x.ArchivedAt, project.ArchivedAt)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={project.Id}"))
            : Result.Ok();
    }

    public async Task<Result> SetNameAsync(Guid projectId, Guid orgId, string name, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.OrganizationId == orgId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Name, name)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetDescriptionAsync(Guid projectId, Guid orgId, string? description, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.OrganizationId == orgId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Description, description)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetSettingsAsync(Guid projectId, Guid orgId, JsonObject? settings, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.OrganizationId == orgId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Settings, settings)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetArchivedAsync(Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.OrganizationId == orgId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.ArchivedAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }

    public async Task<Result> ClearArchivedAsync(Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.OrganizationId == orgId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.ArchivedAt, (DateTime?)null)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}"))
            : Result.Ok();
    }

    public async Task<Result> SoftDeleteAsync(Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.OrganizationId == orgId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DeletedAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}"))
            : Result.Ok();
    }

    public async Task<Result<int>> CountByOrgAsync(Guid orgId, CancellationToken ct = default)
    {
        var count = await _context.Projects
            .Where(x => x.OrganizationId == orgId && x.DeletedAt == null)
            .CountAsync(ct);

        return Result.Ok(count);
    }

    // ---------- Super-admin (platform-wide) ----------

    public async Task<Result<IReadOnlyList<SuperProjectListItemDto>>> ListSuperAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        Guid? organizationId,
        CancellationToken ct = default)
    {
        var query = BuildSuperQuery(search, includeDeleted, organizationId);

        var items = await query
            .OrderBy(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Join(_context.Organizations,
                p => p.OrganizationId,
                o => o.Id,
                (p, o) => new SuperProjectListItemDto(
                    p.Id,
                    p.Name,
                    p.Key,
                    p.Description,
                    p.OrganizationId,
                    o.Name,
                    o.Slug,
                    _context.ProjectMembers.Count(m => m.ProjectId == p.Id),
                    p.CreatedAt,
                    p.DeletedAt,
                    p.ArchivedAt))
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<SuperProjectListItemDto>>(items);
    }

    public async Task<Result<int>> CountSuperAsync(
        string? search,
        bool includeDeleted,
        Guid? organizationId,
        CancellationToken ct = default)
    {
        var count = await BuildSuperQuery(search, includeDeleted, organizationId).CountAsync(ct);
        return Result.Ok(count);
    }

    public async Task<Result<SuperProjectDetailsDto>> GetSuperDetailsAsync(Guid projectId, CancellationToken ct = default)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .Where(p => p.Id == projectId)
            .Join(_context.Organizations,
                p => p.OrganizationId,
                o => o.Id,
                (p, o) => new
                {
                    p.Id, p.Name, p.Key, p.Description,
                    OrganizationId = o.Id,
                    OrganizationName = o.Name,
                    OrganizationSlug = o.Slug,
                    p.CreatedByUserId,
                    p.CreatedAt, p.UpdatedAt, p.DeletedAt, p.ArchivedAt,
                    SdkClientCount = _context.ApiClients.Count(s => s.ProjectId == p.Id),
                    ProcessDefinitionCount = _context.ProcessDefinitions.Count(d => d.ProjectId == p.Id),
                    EventDefinitionCount = _context.EventDefinitions.Count(d => d.ProjectId == p.Id)
                })
            .FirstOrDefaultAsync(ct);

        if (project is null)
        {
            return Result.Fail<SuperProjectDetailsDto>(ApplicationErrors.EntityNotFound("Project", $"id={projectId}"));
        }

        var members = await _context.ProjectMembers
            .AsNoTracking()
            .Where(m => m.ProjectId == projectId)
            .Join(_context.Users,
                m => m.UserId,
                u => u.Id,
                (m, u) => new SuperProjectMemberDto(
                    u.Id,
                    u.Email,
                    u.DisplayName,
                    u.AvatarUrl,
                    m.Role,
                    m.JoinedAt))
            .OrderBy(x => x.Email)
            .ToListAsync(ct);

        return Result.Ok(new SuperProjectDetailsDto(
            project.Id,
            project.Name,
            project.Key,
            project.Description,
            project.OrganizationId,
            project.OrganizationName,
            project.OrganizationSlug,
            project.CreatedByUserId,
            project.CreatedAt,
            project.UpdatedAt,
            project.DeletedAt,
            project.ArchivedAt,
            project.SdkClientCount,
            project.ProcessDefinitionCount,
            project.EventDefinitionCount,
            members));
    }

    public async Task<Result<int>> CountAllAsync(bool includeDeleted, CancellationToken ct = default)
    {
        var query = _context.Projects.AsNoTracking();
        if (!includeDeleted)
        {
            query = query.Where(x => x.DeletedAt == null);
        }

        var count = await query.CountAsync(ct);
        return Result.Ok(count);
    }

    private IQueryable<ProjectEntity> BuildSuperQuery(string? search, bool includeDeleted, Guid? organizationId)
    {
        var query = _context.Projects.AsNoTracking();

        if (!includeDeleted)
        {
            query = query.Where(x => x.DeletedAt == null);
        }

        if (organizationId is not null)
        {
            query = query.Where(x => x.OrganizationId == organizationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.Name, term) ||
                EF.Functions.ILike(x.Key, term));
        }

        return query;
    }
}
