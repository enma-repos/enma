using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.SdkClients;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class SdkClientsService : ISdkClientsService
{
    private readonly ISdkClientsRepository _sdkClientsRepository;

    public SdkClientsService(ISdkClientsRepository sdkClientsRepository)
    {
        _sdkClientsRepository = sdkClientsRepository;
    }

    public async Task<Result<SdkClientDto>> CreateAsync(
        CreateSdkClientDto dto, 
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var modelRes = SdkClient.Create(
            id: Guid.NewGuid(),
            projectId: dto.ProjectId,
            name: dto.Name,
            description: dto.Description,
            type: dto.Type,
            settings: dto.Settings,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<SdkClientDto>(modelRes.Errors);
        }

        var res = await _sdkClientsRepository.CreateAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<SdkClientDto>(res.Errors);
    }

    public async Task<Result<SdkClientDto>> GetByIdAsync(
        Guid clientId, 
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.GetByIdAsync(clientId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<SdkClientDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<SdkClientDto>>> ListByProjectAsync(
        Guid projectId, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.ListByProjectAsync(projectId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<SdkClientDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<SdkClientDto>>(res.Errors);
    }

    public async Task<Result> SetNameAsync(
        Guid clientId, 
        SetSdkClientNameDto dto, 
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetNameAsync(clientId, dto.Name, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetSettingsAsync(
        Guid clientId, 
        SetSdkClientSettingsDto dto, 
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetSettingsAsync(clientId, dto.Settings, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetTypeAsync(
        Guid clientId, 
        SetSdkClientTypeDto dto, 
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetTypeAsync(clientId, dto.Type, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetDisabledAsync(
        Guid clientId, 
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetDisabledAsync(clientId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> ClearDisabledAsync(
        Guid clientId, 
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.ClearDisabledAsync(clientId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }
}
