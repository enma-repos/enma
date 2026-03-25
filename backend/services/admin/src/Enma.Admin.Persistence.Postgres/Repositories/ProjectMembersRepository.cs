using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using Enma.Common.Enums;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class ProjectMembersRepository : IProjectMembersRepository
{
    private readonly PostgresDbContext _context;

    public ProjectMembersRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProjectMember>> AddAsync(ProjectMember member, CancellationToken ct = default)
    {
        _context.ProjectMembers.Add(member.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(member);
    }

    public async Task<Result<ProjectMember>> GetAsync(Guid projectId, Guid orgId, Guid userId, CancellationToken ct = default)
    {
        var entity = await _context.ProjectMembers
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.UserId == userId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .Select(x => SelectWithUser(x))
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<ProjectMember>(ApplicationErrors.EntityNotFound("ProjectMember", $"projectId={projectId}, userId={userId}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<IReadOnlyList<ProjectMember>>> ListByProjectAsync(Guid projectId, Guid orgId, int offset, int limit, CancellationToken ct = default)
    {
        var entities = await _context.ProjectMembers
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .Skip(offset)
            .Take(limit)
            .Select(x => SelectWithUser(x))
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<ProjectMember>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> SetRoleAsync(Guid projectId, Guid orgId, Guid userId, ProjectRole role, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ProjectMembers
            .Where(x => x.ProjectId == projectId && x.UserId == userId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Role, role)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("ProjectMember", $"projectId={projectId}, userId={userId}"))
            : Result.Ok();
    }

    public async Task<Result> RemoveAsync(Guid projectId, Guid orgId, Guid userId, CancellationToken ct = default)
    {
        var affected = await _context.ProjectMembers
            .Where(x => x.ProjectId == projectId && x.UserId == userId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteDeleteAsync(ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("ProjectMember", $"projectId={projectId}, userId={userId}"))
            : Result.Ok();
    }

    private static ProjectMemberEntity SelectWithUser(ProjectMemberEntity x)
        => new()
        {
            ProjectId = x.ProjectId,
            UserId = x.UserId,
            Role = x.Role,
            JoinedAt = x.JoinedAt,
            UpdatedAt = x.UpdatedAt,
            User = x.User == null ? null : new UserEntity
            {
                Id = x.User.Id,
                Email = x.User.Email,
                DisplayName = x.User.DisplayName,
                AvatarUrl = x.User.AvatarUrl
            }
        };
}
