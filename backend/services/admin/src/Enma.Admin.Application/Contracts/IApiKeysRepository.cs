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
    Task<Result<ApiKey>> CreateAsync(ApiKey key, CancellationToken ct = default);

    /// <summary>Gets an API key by id.</summary>
    Task<Result<ApiKey>> GetByIdAsync(Guid apiKeyId, CancellationToken ct = default);

    /// <summary>Lists API keys for an SDK client (paged by offset/limit).</summary>
    Task<Result<IReadOnlyList<ApiKey>>> ListBySdkClientAsync(Guid sdkClientId, int offset, int limit, CancellationToken ct = default);

    /// <summary>
    /// Lists active (non-revoked) API keys by prefix for authentication purposes.
    /// Implementations should return only a small subset (bounded by <paramref name="limit"/>).
    /// </summary>
    Task<Result<IReadOnlyList<ApiKey>>> ListActiveByPrefixAsync(string keyPrefix, int limit, CancellationToken ct = default);

    /// <summary>Updates <see cref="ApiKey.LastUsedAt"/> only.</summary>
    Task<Result> UpdateLastUsedAsync(Guid apiKeyId, CancellationToken ct = default);

    /// <summary>Updates <see cref="ApiKey.RevokedAt"/> only.</summary>
    Task<Result> UpdateRevokedAsync(Guid apiKeyId, CancellationToken ct = default);
}