namespace Enma.Auth.Application.Dto.Auth;

public sealed record StartExternalAuthRequestDto(
    string Provider,
    string? SuccessUrl = null);