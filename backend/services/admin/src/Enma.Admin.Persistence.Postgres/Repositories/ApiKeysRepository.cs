using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class ApiKeysRepository : IApiKeysRepository
{
    private readonly PostgresDbContext _context;

    public ApiKeysRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ApiKey>> CreateAsync(ApiKey key, CancellationToken ct = default)
    {
        _context.ApiKeys.Add(key.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(key);
    }

    public async Task<Result<ApiKey>> GetByIdAsync(Guid apiKeyId, CancellationToken ct = default)
    {
        var entity = await _context.ApiKeys
            .AsNoTracking()
            .Where(x => x.Id == apiKeyId)
            .Select(x => new ApiKeyEntity
            {
                Id = x.Id,
                SdkClientId = x.SdkClientId,
                KeyPrefix = x.KeyPrefix,
                KeyHash = x.KeyHash,
                SentEventsCount = x.SentEventsCount,
                CreatedAt = x.CreatedAt,
                LastUsedAt = x.LastUsedAt,
                RevokedAt = x.RevokedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<ApiKey>(ApplicationErrors.EntityNotFound("ApiKey", $"id={apiKeyId}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<IReadOnlyList<ApiKey>>> ListBySdkClientAsync(Guid sdkClientId, int offset, int limit, CancellationToken ct = default)
    {
        var entities = await _context.ApiKeys
            .AsNoTracking()
            .Where(x => x.SdkClientId == sdkClientId)
            .Skip(offset)
            .Take(limit)
            .Select(x => new ApiKeyEntity
            {
                Id = x.Id,
                SdkClientId = x.SdkClientId,
                KeyPrefix = x.KeyPrefix,
                KeyHash = x.KeyHash,
                SentEventsCount = x.SentEventsCount,
                CreatedAt = x.CreatedAt,
                LastUsedAt = x.LastUsedAt,
                RevokedAt = x.RevokedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<ApiKey>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<IReadOnlyList<ApiKey>>> ListActiveByPrefixAsync(string keyPrefix, int limit, CancellationToken ct = default)
    {
        var entities = await _context.ApiKeys
            .AsNoTracking()
            .Where(x => x.KeyPrefix == keyPrefix && x.RevokedAt == null)
            .Take(limit)
            .Select(x => new ApiKeyEntity
            {
                Id = x.Id,
                SdkClientId = x.SdkClientId,
                KeyPrefix = x.KeyPrefix,
                KeyHash = x.KeyHash,
                SentEventsCount = x.SentEventsCount,
                CreatedAt = x.CreatedAt,
                LastUsedAt = x.LastUsedAt,
                RevokedAt = x.RevokedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<ApiKey>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> UpdateLastUsedAsync(Guid apiKeyId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiKeys
            .Where(x => x.Id == apiKeyId && x.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.LastUsedAt, now), ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("ApiKey", $"id={apiKeyId}")) 
            : Result.Ok();
    }

    public async Task<Result> UpdateRevokedAsync(Guid apiKeyId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiKeys
            .Where(x => x.Id == apiKeyId && x.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, now), ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("ApiKey", $"id={apiKeyId}")) 
            : Result.Ok();
    }
}
