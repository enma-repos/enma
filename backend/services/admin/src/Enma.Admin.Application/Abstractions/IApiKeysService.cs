using Enma.Admin.Application.Dto.ApiKeys;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IApiKeysService
{
    Task<Result<ApiKeyFirstCreationDto>> CreateAsync(Guid sdkClientId, CancellationToken ct = default);
    Task<Result<ApiKeyDto>> GetByIdAsync(Guid apiKeyId, CancellationToken ct = default);

    Task<Result<IReadOnlyList<ApiKeyDto>>> ListBySdkClientAsync(
        Guid sdkClientId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result<IReadOnlyList<ApiKeyDto>>> ListActiveByPrefixAsync(
        string keyPrefix,
        int limit,
        CancellationToken ct = default);

    Task<Result> UpdateLastUsedAsync(Guid apiKeyId, CancellationToken ct = default);
    Task<Result> UpdateRevokedAsync(Guid apiKeyId, CancellationToken ct = default);
}
