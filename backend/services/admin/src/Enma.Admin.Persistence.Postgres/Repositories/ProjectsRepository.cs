using System.Text.Json.Nodes;
using Enma.Admin.Application.Contracts;
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

    public async Task<Result<Project>> GetByIdAsync(Guid projectId, CancellationToken ct = default)
    {
        var entity = await _context.Projects
            .AsNoTracking()
            .Where(x => x.Id == projectId && x.DeletedAt == null)
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

    public async Task<Result> SetNameAsync(Guid projectId, string name, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Name, name)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetDescriptionAsync(Guid projectId, string? description, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Description, description)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetSettingsAsync(Guid projectId, JsonObject? settings, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Settings, settings)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetArchivedAsync(Guid projectId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.ArchivedAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }

    public async Task<Result> ClearArchivedAsync(Guid projectId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Projects
            .Where(x => x.Id == projectId && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.ArchivedAt, (DateTime?)null)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Project", $"id={projectId}")) 
            : Result.Ok();
    }
}
