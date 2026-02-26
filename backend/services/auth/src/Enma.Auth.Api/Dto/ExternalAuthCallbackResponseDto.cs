using Enma.Auth.Application.Dto.Auth;

namespace Enma.Auth.Api.Dto;

public sealed record ExternalAuthCallbackResponseDto(
    AuthTokensDto Tokens,
    string SuccessUrl);

