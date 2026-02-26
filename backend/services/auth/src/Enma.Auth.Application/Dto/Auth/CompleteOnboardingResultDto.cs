using Enma.Auth.Application.Dto.AdminUsers;
using Enma.Auth.Application.Dto.Responses;

namespace Enma.Auth.Application.Dto.Auth;

public sealed record CompleteOnboardingResultDto(
    GetAccountResponseDto Account,
    AdminUserDto User);

