using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.SdkClients;
using Enma.Admin.Application.Models;
using Enma.Common.Models;
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

    public async Task<Result<PaginatedResult<SdkClientDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.ListByProjectAsync(projectId, orgId, page, pageSize, search, ct);
        if (res.IsFailed) return Result.Fail<PaginatedResult<SdkClientDto>>(res.Errors);

        var countRes = await _sdkClientsRepository.CountByProjectAsync(projectId, orgId, search, ct);
        if (countRes.IsFailed) return Result.Fail<PaginatedResult<SdkClientDto>>(countRes.Errors);

        var items = res.Value.Select(x => x.ToDto()).ToList();
        return PaginatedResult<SdkClientDto>.Create(items, countRes.Value, page, pageSize);
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

    public async Task<Result> SetDescriptionAsync(
        Guid clientId,
        Guid projectId,
        Guid orgId,
        SetSdkClientDescriptionDto dto,
        CancellationToken ct = default)
    {
        var res = await _sdkClientsRepository.SetDescriptionAsync(clientId, projectId, orgId, dto.Description, ct);
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
