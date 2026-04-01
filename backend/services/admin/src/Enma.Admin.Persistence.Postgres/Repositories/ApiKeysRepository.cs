using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.ApiKeys;
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

    public async Task<Result<ApiKey>> CreateAsync(ApiKey key, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var clientBelongsToProject = await _context.ApiClients
            .AnyAsync(c => c.Id == sdkClientId && c.ProjectId == projectId, ct);
        if (!clientBelongsToProject)
            return Result.Fail<ApiKey>(ApplicationErrors.EntityNotFound("SdkClient", $"id={sdkClientId}, projectId={projectId}"));

        var projectBelongsToOrg = await _context.Projects
            .AnyAsync(p => p.Id == projectId && p.OrganizationId == orgId && p.DeletedAt == null, ct);
        if (!projectBelongsToOrg)
            return Result.Fail<ApiKey>(ApplicationErrors.EntityNotFound("Project", $"id={projectId}, orgId={orgId}"));

        _context.ApiKeys.Add(key.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(key);
    }

    public async Task<Result<ApiKey>> GetByIdAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var entity = await _context.ApiKeys
            .AsNoTracking()
            .Where(x => x.Id == apiKeyId && x.SdkClientId == sdkClientId
                && _context.ApiClients.Any(c => c.Id == sdkClientId && c.ProjectId == projectId)
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
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

    public async Task<Result<IReadOnlyList<ApiKey>>> ListBySdkClientAsync(Guid sdkClientId, Guid projectId, Guid orgId, int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var query = _context.ApiKeys
            .AsNoTracking()
            .Where(x => x.SdkClientId == sdkClientId
                && _context.ApiClients.Any(c => c.Id == sdkClientId && c.ProjectId == projectId)
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search}%";
            query = query.Where(x => EF.Functions.ILike(x.KeyPrefix, term));
        }

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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

    public async Task<Result<int>> CountBySdkClientAsync(Guid sdkClientId, Guid projectId, Guid orgId, string? search = null, CancellationToken ct = default)
    {
        var query = _context.ApiKeys
            .AsNoTracking()
            .Where(x => x.SdkClientId == sdkClientId
                && _context.ApiClients.Any(c => c.Id == sdkClientId && c.ProjectId == projectId)
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search}%";
            query = query.Where(x => EF.Functions.ILike(x.KeyPrefix, term));
        }

        var count = await query.CountAsync(ct);
        return Result.Ok(count);
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

    public async Task<Result> UpdateLastUsedAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiKeys
            .Where(x => x.Id == apiKeyId && x.SdkClientId == sdkClientId && x.RevokedAt == null
                && _context.ApiClients.Any(c => c.Id == sdkClientId && c.ProjectId == projectId)
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.LastUsedAt, now), ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("ApiKey", $"id={apiKeyId}"))
            : Result.Ok();
    }

    public async Task<Result> UpdateRevokedAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiKeys
            .Where(x => x.Id == apiKeyId && x.SdkClientId == sdkClientId && x.RevokedAt == null
                && _context.ApiClients.Any(c => c.Id == sdkClientId && c.ProjectId == projectId)
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, now), ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("ApiKey", $"id={apiKeyId}"))
            : Result.Ok();
    }

    public async Task<Result<ApiKeyWithContextDto?>> FindActiveByHashAsync(string keyHash, string keyPrefix, CancellationToken ct = default)
    {
        var result = await _context.ApiKeys
            .AsNoTracking()
            .Where(k => k.KeyPrefix == keyPrefix && k.KeyHash == keyHash && k.RevokedAt == null)
            .Join(_context.ApiClients, k => k.SdkClientId, c => c.Id, (k, c) => new { k, c })
            .Join(_context.Projects, kc => kc.c.ProjectId, p => p.Id, (kc, p) => new { kc.k, kc.c, p })
            .Where(x => x.p.DeletedAt == null)
            .Select(x => new ApiKeyWithContextDto(
                x.k.Id,
                x.c.Id,
                x.p.Id,
                x.p.OrganizationId))
            .FirstOrDefaultAsync(ct);

        return Result.Ok(result);
    }
}
