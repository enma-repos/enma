using Enma.Admin.Application.Dto.ApiKeys;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for domain <see cref="ApiKey"/>.
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// when the entity does not exist.
/// </summary>
public interface IApiKeysRepository
{
    /// <summary>Creates a new API key record (hash/prefix must already be computed by application/infrastructure).</summary>
    Task<Result<ApiKey>> CreateAsync(ApiKey key, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Gets an API key by id.</summary>
    Task<Result<ApiKey>> GetByIdAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Lists API keys for an SDK client (paged).</summary>
    Task<Result<IReadOnlyList<ApiKey>>> ListBySdkClientAsync(Guid sdkClientId, Guid projectId, Guid orgId, int page, int pageSize, string? search = null, CancellationToken ct = default);

    /// <summary>Returns the total count of API keys for an SDK client.</summary>
    Task<Result<int>> CountBySdkClientAsync(Guid sdkClientId, Guid projectId, Guid orgId, string? search = null, CancellationToken ct = default);

    /// <summary>
    /// Lists active (non-revoked) API keys by prefix for authentication purposes.
    /// Implementations should return only a small subset (bounded by <paramref name="limit"/>).
    /// </summary>
    Task<Result<IReadOnlyList<ApiKey>>> ListActiveByPrefixAsync(string keyPrefix, int limit, CancellationToken ct = default);

    /// <summary>Updates <see cref="ApiKey.LastUsedAt"/> only.</summary>
    Task<Result> UpdateLastUsedAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Updates <see cref="ApiKey.RevokedAt"/> only.</summary>
    Task<Result> UpdateRevokedAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>
    /// Finds an active (non-revoked) API key by its hash and prefix, returning the key context (orgId, projectId, sdkClientId).
    /// Used for SDK API key authentication.
    /// </summary>
    Task<Result<ApiKeyWithContextDto?>> FindActiveByHashAsync(string keyHash, string keyPrefix, CancellationToken ct = default);

    /// <summary>Counts all API keys across the platform (used for super-admin overview).</summary>
    Task<Result<int>> CountAllAsync(CancellationToken ct = default);
}