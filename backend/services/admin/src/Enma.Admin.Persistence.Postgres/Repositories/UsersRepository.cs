using Enma.Admin.Application.Contracts;
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
}
