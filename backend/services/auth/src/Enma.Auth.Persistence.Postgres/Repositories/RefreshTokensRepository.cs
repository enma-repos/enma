using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Application.Models;
using Enma.Auth.Persistence.Postgres.Connection;
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
}
