using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.OrganizationInvites;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class OrganizationInvitesService : IOrganizationInvitesService
{
    private const int InviteLifeTimeInDays = 30;
    
    private readonly IOrganizationInvitesRepository _organizationInvitesRepository;
    private readonly ISecretService _secretService;

    public OrganizationInvitesService(
        IOrganizationInvitesRepository organizationInvitesRepository, 
        ISecretService secretService)
    {
        _organizationInvitesRepository = organizationInvitesRepository;
        _secretService = secretService;
    }

    public async Task<Result<OrganizationInviteDto>> CreateAsync(
        CreateOrganizationInviteDto dto, 
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var expiresAt = now + TimeSpan.FromDays(InviteLifeTimeInDays);
        var material = _secretService.Generate(tokenPrefix: "inv");

        var modelRes = OrganizationInvite.Create(
            id: Guid.NewGuid(),
            orgId: dto.OrganizationId,
            email: dto.TargetEmail,
            role: dto.Role,
            tokenHash: material.Hash,
            createdByUserId: dto.CreatedByUserId,
            acceptedUserId: null,
            createdAt: now,
            expiresAt: expiresAt,
            acceptedAt: null);

        if (modelRes.IsFailed)
        {
            return Result.Fail<OrganizationInviteDto>(modelRes.Errors);
        }
        
        // TODO: send invite with token (check that user email exists in db)

        var res = await _organizationInvitesRepository.CreateAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationInviteDto>(res.Errors);
    }

    public async Task<Result<OrganizationInviteDto>> GetByIdAsync(
        Guid inviteId, 
        CancellationToken ct = default)
    {
        var res = await _organizationInvitesRepository.GetByIdAsync(inviteId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationInviteDto>(res.Errors);
    }

    public async Task<Result<OrganizationInviteDto>> GetActiveByOrgAndEmailAsync(
        Guid orgId, 
        string email, 
        CancellationToken ct = default)
    {
        var res = await _organizationInvitesRepository.GetActiveByOrgAndEmailAsync(orgId, email, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationInviteDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<OrganizationInviteDto>>> ListActiveByOrgAsync(
        Guid orgId, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _organizationInvitesRepository.ListActiveByOrgAsync(orgId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<OrganizationInviteDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<OrganizationInviteDto>>(res.Errors);
    }

    public async Task<Result> SetAcceptedAsync(
        Guid inviteId, 
        SetInviteAcceptedDto dto, 
        CancellationToken ct = default)
    {
        // TODO: add user to members by transaction
        
        var res = await _organizationInvitesRepository.SetAcceptedAsync(inviteId, dto.AcceptedUserId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }
}
