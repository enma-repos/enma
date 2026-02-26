using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Application.Models;
using Enma.Auth.Persistence.Postgres.Connection;
using Enma.Auth.Persistence.Postgres.Entities;
using Enma.Auth.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Auth.Persistence.Postgres.Repositories;

internal sealed class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly PostgresDbContext _context;

    public RefreshTokensRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<RefreshToken>> GetByTokenHashAsync(string tokenHash, CancellationToken ct)
    {
        var entity = await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);

        return entity is null
            ? Result.Fail<RefreshToken>(ApplicationErrors.EntityNotFound("RefreshToken", $"tokenHash={tokenHash}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<List<RefreshToken>>> GetByAccountIdAsync(Guid accountId, CancellationToken ct)
    {
        var entities = await _context.RefreshTokens
            .AsNoTracking()
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);

        return Result.Ok(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> UpdateLastLoginAsync(Guid tokenId, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.RefreshTokens
            .Where(x => x.Id == tokenId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.LastUsedAt, now), ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("RefreshToken", $"id={tokenId}"))
            : Result.Ok();
    }

    public async Task<Result<Guid>> CreateAsync(RefreshToken token, CancellationToken ct)
    {
        _context.RefreshTokens.Add(new RefreshTokenEntity
        {
            Id = token.Id,
            AccountId = token.AccountId,
            Account = null!,
            TokenHash = token.TokenHash,
            CreatedAt = token.CreatedAt,
            ExpiresAt = token.ExpiresAt,
            LastUsedAt = token.LastUsedAt
        });

        await _context.SaveChangesAsync(ct);
        return Result.Ok(token.Id);
    }

    public async Task<Result> DeleteAsync(Guid tokenId, CancellationToken ct)
    {
        var affected = await _context.RefreshTokens
            .Where(x => x.Id == tokenId)
            .ExecuteDeleteAsync(ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("RefreshToken", $"id={tokenId}"))
            : Result.Ok();
    }
}
