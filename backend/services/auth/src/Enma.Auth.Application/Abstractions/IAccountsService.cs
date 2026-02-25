using Enma.Auth.Application.Dto.Responses;
using Enma.Common.Enums;
using FluentResults;

namespace Enma.Auth.Application.Abstractions;

public interface IAccountsService
{
    Task<Result<GetAccountResponseDto>> GetByIdAsync(Guid accountId, CancellationToken ct = default);
    Task<Result<GetAccountResponseDto>> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Result> UpdateStatusAsync(Guid accountId, AccountStatus status, CancellationToken ct = default);
}
