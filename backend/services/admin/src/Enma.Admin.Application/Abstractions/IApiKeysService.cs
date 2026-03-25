using Enma.Admin.Application.Dto.ApiKeys;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IApiKeysService
{
    Task<Result<ApiKeyFirstCreationDto>> CreateAsync(Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result<ApiKeyDto>> GetByIdAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default);

    Task<Result<IReadOnlyList<ApiKeyDto>>> ListBySdkClientAsync(
        Guid sdkClientId,
        Guid projectId,
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result<IReadOnlyList<ApiKeyDto>>> ListActiveByPrefixAsync(
        string keyPrefix,
        int limit,
        CancellationToken ct = default);

    Task<Result> UpdateLastUsedAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result> UpdateRevokedAsync(Guid apiKeyId, Guid sdkClientId, Guid projectId, Guid orgId, CancellationToken ct = default);
}
