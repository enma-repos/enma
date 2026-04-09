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

internal sealed class UsersRepository : IUsersRepository
{
    private readonly PostgresDbContext _context;

    public UsersRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> GetOrCreateAsync(User user, CancellationToken ct = default)
    {
        try
        {
            _context.Users.Add(user.ToEntity());
            await _context.SaveChangesAsync(ct);

            return Result.Ok(user);
        }
        catch (DbUpdateException)
        {
            var existingResult = await GetByIdAsync(user.Id, ct);
            return existingResult.IsSuccess
                ? Result.Ok(existingResult.Value)
                : Result.Fail<User>(ApplicationErrors.Conflict("Failed to create user."));
        }
    }

    public async Task<Result<User>> GetByIdAsync(Guid userId, CancellationToken ct = default)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => new UserEntity
            {
                Id = x.Id,
                Email = x.Email,
                DisplayName = x.DisplayName,
                AvatarUrl = x.AvatarUrl,
                Locale = x.Locale,
                Timezone = x.Timezone,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<User>(ApplicationErrors.EntityNotFound("User", $"id={userId}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .Where(x => x.Email == email)
            .Select(x => new UserEntity
            {
                Id = x.Id,
                Email = x.Email,
                DisplayName = x.DisplayName,
                AvatarUrl = x.AvatarUrl,
                Locale = x.Locale,
                Timezone = x.Timezone,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<User>(ApplicationErrors.EntityNotFound("User", $"email={email}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<bool>> ExistsAsync(Guid userId, CancellationToken ct = default)
    {
        var exists = await _context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == userId, ct);

        return Result.Ok(exists);
    }

    public async Task<Result> UpdateAsync(User user, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Users
            .Where(x => x.Id == user.Id)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DisplayName, user.DisplayName)
                    .SetProperty(x => x.AvatarUrl, user.AvatarUrl)
                    .SetProperty(x => x.Locale, user.Locale)
                    .SetProperty(x => x.Timezone, user.Timezone)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("User", $"id={user.Id}"))
            : Result.Ok();
    }

    public async Task<Result> SetDisplayNameAsync(Guid userId, string displayName, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DisplayName, displayName)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("User", $"id={userId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetAvatarUrlAsync(Guid userId, string? avatarUrl, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.AvatarUrl, avatarUrl)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("User", $"id={userId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetLocaleAsync(Guid userId, string? locale, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Locale, locale)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("User", $"id={userId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetTimezoneAsync(Guid userId, string? timezone, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Timezone, timezone)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("User", $"id={userId}")) 
            : Result.Ok();
    }

    public async Task<Result> SoftDeleteAsync(Guid userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DeletedAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("User", $"id={userId}"))
            : Result.Ok();
    }

    // ---------- Super-admin (platform-wide) ----------

    public async Task<Result<IReadOnlyList<SuperUserListItemDto>>> ListSuperAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        CancellationToken ct = default)
    {
        var query = BuildSuperQuery(search, includeDeleted);

        var items = await query
            .OrderBy(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SuperUserListItemDto(
                x.Id,
                x.Email,
                x.DisplayName,
                x.AvatarUrl,
                _context.OrganizationMembers.Count(m => m.UserId == x.Id),
                _context.ProjectMembers.Count(m => m.UserId == x.Id),
                x.CreatedAt,
                x.DeletedAt))
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<SuperUserListItemDto>>(items);
    }

    public async Task<Result<int>> CountSuperAsync(
        string? search,
        bool includeDeleted,
        CancellationToken ct = default)
    {
        var count = await BuildSuperQuery(search, includeDeleted).CountAsync(ct);
        return Result.Ok(count);
    }

    public async Task<Result<SuperUserDetailsDto>> GetSuperDetailsAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => new { x.Id, x.Email, x.DisplayName, x.AvatarUrl, x.Locale, x.Timezone, x.CreatedAt, x.UpdatedAt, x.DeletedAt })
            .FirstOrDefaultAsync(ct);

        if (user is null)
        {
            return Result.Fail<SuperUserDetailsDto>(ApplicationErrors.EntityNotFound("User", $"id={userId}"));
        }

        var organizations = await _context.OrganizationMembers
            .AsNoTracking()
            .Where(m => m.UserId == userId)
            .Join(_context.Organizations,
                m => m.OrganizationId,
                o => o.Id,
                (m, o) => new SuperUserOrganizationMembershipDto(
                    o.Id,
                    o.Name,
                    o.Slug,
                    m.Role,
                    m.JoinedAt))
            .OrderBy(x => x.OrganizationName)
            .ToListAsync(ct);

        var projects = await _context.ProjectMembers
            .AsNoTracking()
            .Where(m => m.UserId == userId)
            .Join(_context.Projects,
                m => m.ProjectId,
                p => p.Id,
                (m, p) => new { Member = m, Project = p })
            .Join(_context.Organizations,
                mp => mp.Project.OrganizationId,
                o => o.Id,
                (mp, o) => new SuperUserProjectMembershipDto(
                    mp.Project.Id,
                    mp.Project.Name,
                    mp.Project.Key,
                    o.Id,
                    o.Name,
                    mp.Member.Role,
                    mp.Member.JoinedAt))
            .OrderBy(x => x.OrganizationName)
            .ThenBy(x => x.ProjectName)
            .ToListAsync(ct);

        return Result.Ok(new SuperUserDetailsDto(
            user.Id,
            user.Email,
            user.DisplayName,
            user.AvatarUrl,
            user.Locale,
            user.Timezone,
            user.CreatedAt,
            user.UpdatedAt,
            user.DeletedAt,
            organizations,
            projects));
    }

    public async Task<Result<int>> CountAllAsync(bool includeDeleted, CancellationToken ct = default)
    {
        var query = _context.Users.AsNoTracking();
        if (!includeDeleted)
        {
            query = query.Where(x => x.DeletedAt == null);
        }

        var count = await query.CountAsync(ct);
        return Result.Ok(count);
    }

    private IQueryable<UserEntity> BuildSuperQuery(string? search, bool includeDeleted)
    {
        var query = _context.Users.AsNoTracking();

        if (!includeDeleted)
        {
            query = query.Where(x => x.DeletedAt == null);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.Email, term) ||
                EF.Functions.ILike(x.DisplayName, term));
        }

        return query;
    }
}
