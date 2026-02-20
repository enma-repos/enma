using Enma.Admin.Application.Dto.SdkClients;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface ISdkClientsService
{
    Task<Result<SdkClientDto>> CreateAsync(CreateSdkClientDto dto, CancellationToken ct = default);
    Task<Result<SdkClientDto>> GetByIdAsync(Guid clientId, CancellationToken ct = default);

    Task<Result<IReadOnlyList<SdkClientDto>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetNameAsync(Guid clientId, SetSdkClientNameDto dto, CancellationToken ct = default);
    Task<Result> SetSettingsAsync(Guid clientId, SetSdkClientSettingsDto dto, CancellationToken ct = default);
    Task<Result> SetTypeAsync(Guid clientId, SetSdkClientTypeDto dto, CancellationToken ct = default);
    Task<Result> SetDisabledAsync(Guid clientId, CancellationToken ct = default);
    Task<Result> ClearDisabledAsync(Guid clientId, CancellationToken ct = default);
}
