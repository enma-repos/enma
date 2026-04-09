using Enma.Auth.Application.Dto.AdminUsers;
using Enma.Auth.Application.Dto.Responses;
using Enma.Common.Enums;

namespace Enma.Auth.Application.Dto.Auth;

public sealed record MeDto(
    GetAccountResponseDto Account,
    AdminUserDto? User,
    UserRole Role);
