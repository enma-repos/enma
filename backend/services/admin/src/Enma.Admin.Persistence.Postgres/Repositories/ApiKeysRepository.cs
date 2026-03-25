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

    public async Task<Result<IReadOnlyList<ApiKey>>> ListBySdkClientAsync(Guid sdkClientId, Guid projectId, Guid orgId, int offset, int limit, CancellationToken ct = default)
    {
        var entities = await _context.ApiKeys
            .AsNoTracking()
            .Where(x => x.SdkClientId == sdkClientId
                && _context.ApiClients.Any(c => c.Id == sdkClientId && c.ProjectId == projectId)
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
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
}
