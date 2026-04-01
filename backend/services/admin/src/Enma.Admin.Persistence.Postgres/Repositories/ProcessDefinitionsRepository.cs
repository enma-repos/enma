using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class ProcessDefinitionsRepository : IProcessDefinitionsRepository
{
    private readonly PostgresDbContext _context;

    public ProcessDefinitionsRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProcessDefinition>> CreateAsync(ProcessDefinition model, Guid orgId, CancellationToken ct = default)
    {
        var projectBelongsToOrg = await _context.Projects
            .AnyAsync(p => p.Id == model.ProjectId && p.OrganizationId == orgId && p.DeletedAt == null, ct);

        if (!projectBelongsToOrg)
        {
            return Result.Fail<ProcessDefinition>(ApplicationErrors.EntityNotFound("Project", $"id={model.ProjectId}, orgId={orgId}"));
        }

        _context.ProcessDefinitions.Add(model.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(model);
    }

    public async Task<Result<ProcessDefinition>> GetByIdAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var entity = await _context.ProcessDefinitions
            .AsNoTracking()
            .Where(x => x.Id == id && x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .Select(x => new ProcessDefinitionEntity
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Key = x.Key,
                Type = x.Type,
                Description = x.Description,
                CreatedByUserId = x.CreatedByUserId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<ProcessDefinition>(ApplicationErrors.EntityNotFound("ProcessDefinition", $"id={id}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<ProcessDefinition>> GetByProjectAndKeyAsync(Guid projectId, Guid orgId, string key,
        CancellationToken ct = default)
    {
        var entity = await _context.ProcessDefinitions
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.Key == key && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .Select(x => new ProcessDefinitionEntity
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Key = x.Key,
                Type = x.Type,
                Description = x.Description,
                CreatedByUserId = x.CreatedByUserId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<ProcessDefinition>(
                ApplicationErrors.EntityNotFound("ProcessDefinition", $"projectId={projectId}, key={key}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<int>> CountByProjectAsync(Guid projectId, Guid orgId, string? search = null, CancellationToken ct = default)
    {
        var query = _context.ProcessDefinitions
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search}%";
            query = query.Where(x => EF.Functions.ILike(x.Name, term) || EF.Functions.ILike(x.Key, term) || EF.Functions.ILike(x.Description ?? "", term));
        }

        var count = await query.CountAsync(ct);
        return Result.Ok(count);
    }

    public async Task<Result<IReadOnlyList<ProcessDefinition>>> ListByProjectAsync(Guid projectId, Guid orgId, int page,
        int pageSize, string? search = null, CancellationToken ct = default)
    {
        var query = _context.ProcessDefinitions
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search}%";
            query = query.Where(x => EF.Functions.ILike(x.Name, term) || EF.Functions.ILike(x.Key, term) || EF.Functions.ILike(x.Description ?? "", term));
        }

        var entities = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProcessDefinitionEntity
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Key = x.Key,
                Type = x.Type,
                Description = x.Description,
                CreatedByUserId = x.CreatedByUserId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<ProcessDefinition>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> SetNameAsync(Guid id, Guid projectId, Guid orgId, string name, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ProcessDefinitions
            .Where(x => x.Id == id && x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Name, name)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("ProcessDefinition", $"id={id}"))
            : Result.Ok();
    }

    public async Task<Result> SetDescriptionAsync(Guid id, Guid projectId, Guid orgId, string? description, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ProcessDefinitions
            .Where(x => x.Id == id && x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Description, description)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("ProcessDefinition", $"id={id}"))
            : Result.Ok();
    }

    public async Task<Result> SoftDeleteAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ProcessDefinitions
            .Where(x => x.Id == id && x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DeletedAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("ProcessDefinition", $"id={id}"))
            : Result.Ok();
    }
}
