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
        Guid orgId,
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

        var res = await _sdkClientsRepository.CreateAsync(modelRes.Value, orgId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<SdkClientDto>(res.Errors);
    }

    public async Task<Result<SdkClientDto>> GetByIdAsync(
        Guid clientId,
        Guid projectId,
        Guid orgId,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.GetByIdAsync(clientId, projectId, orgId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<SdkClientDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<SdkClientDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.ListByProjectAsync(projectId, orgId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<SdkClientDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<SdkClientDto>>(res.Errors);
    }

    public async Task<Result> SetNameAsync(
        Guid clientId,
        Guid projectId,
        Guid orgId,
        SetSdkClientNameDto dto,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetNameAsync(clientId, projectId, orgId, dto.Name, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetSettingsAsync(
        Guid clientId,
        Guid projectId,
        Guid orgId,
        SetSdkClientSettingsDto dto,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetSettingsAsync(clientId, projectId, orgId, dto.Settings, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetTypeAsync(
        Guid clientId,
        Guid projectId,
        Guid orgId,
        SetSdkClientTypeDto dto,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetTypeAsync(clientId, projectId, orgId, dto.Type, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetDisabledAsync(
        Guid clientId,
        Guid projectId,
        Guid orgId,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetDisabledAsync(clientId, projectId, orgId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> ClearDisabledAsync(
        Guid clientId,
        Guid projectId,
        Guid orgId,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.ClearDisabledAsync(clientId, projectId, orgId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }
}
