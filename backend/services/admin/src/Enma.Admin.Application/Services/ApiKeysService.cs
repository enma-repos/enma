using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.ApiKeys;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class ApiKeysService : IApiKeysService
{
    private readonly IApiKeysRepository _apiKeysRepository;
    private readonly ISecretService _secretService;

    public ApiKeysService(
        IApiKeysRepository apiKeysRepository, 
        ISecretService secretService)
    {
        _apiKeysRepository = apiKeysRepository;
        _secretService = secretService;
    }

    public async Task<Result<ApiKeyFirstCreationDto>> CreateAsync(
        Guid sdkClientId, 
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var material = _secretService.Generate(tokenPrefix: "sdk");
        
        var modelRes = ApiKey.Create(
            id: Guid.NewGuid(),
            sdkClientId: sdkClientId,
            keyPrefix: material.Prefix,
            keyHash: material.Hash,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<ApiKeyFirstCreationDto>(modelRes.Errors);
        }

        var res = await _apiKeysRepository.CreateAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto(material.PlainKey))
            : Result.Fail<ApiKeyFirstCreationDto>(res.Errors);
    }

    public async Task<Result<ApiKeyDto>> GetByIdAsync(
        Guid apiKeyId, 
        CancellationToken ct = default)
    {
        var res = await _apiKeysRepository.GetByIdAsync(apiKeyId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ApiKeyDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<ApiKeyDto>>> ListBySdkClientAsync(
        Guid sdkClientId, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _apiKeysRepository.ListBySdkClientAsync(sdkClientId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<ApiKeyDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<ApiKeyDto>>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<ApiKeyDto>>> ListActiveByPrefixAsync(
        string keyPrefix, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _apiKeysRepository.ListActiveByPrefixAsync(keyPrefix, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<ApiKeyDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<ApiKeyDto>>(res.Errors);
    }

    public async Task<Result> UpdateLastUsedAsync(
        Guid apiKeyId, 
        CancellationToken ct = default)
    {
        var res = await _apiKeysRepository.UpdateLastUsedAsync(apiKeyId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> UpdateRevokedAsync(
        Guid apiKeyId, 
        CancellationToken ct = default)
    {
        var res = await _apiKeysRepository.UpdateRevokedAsync(apiKeyId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }
}
