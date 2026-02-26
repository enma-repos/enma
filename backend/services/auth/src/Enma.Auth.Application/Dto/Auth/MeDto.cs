using Enma.Auth.Application.Dto.AdminUsers;
using Enma.Auth.Application.Dto.Responses;

namespace Enma.Auth.Application.Dto.Auth;

public sealed record MeDto(
    GetAccountResponseDto Account,
    AdminUserDto? User);

