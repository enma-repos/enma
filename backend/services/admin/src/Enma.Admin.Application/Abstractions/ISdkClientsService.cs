using Enma.Admin.Application.Dto.SdkClients;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface ISdkClientsService
{
    Task<Result<SdkClientDto>> CreateAsync(CreateSdkClientDto dto, Guid orgId, CancellationToken ct = default);
    Task<Result<SdkClientDto>> GetByIdAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default);

    Task<Result<IReadOnlyList<SdkClientDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetNameAsync(Guid clientId, Guid projectId, Guid orgId, SetSdkClientNameDto dto, CancellationToken ct = default);
    Task<Result> SetSettingsAsync(Guid clientId, Guid projectId, Guid orgId, SetSdkClientSettingsDto dto, CancellationToken ct = default);
    Task<Result> SetTypeAsync(Guid clientId, Guid projectId, Guid orgId, SetSdkClientTypeDto dto, CancellationToken ct = default);
    Task<Result> SetDisabledAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result> ClearDisabledAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default);
}
