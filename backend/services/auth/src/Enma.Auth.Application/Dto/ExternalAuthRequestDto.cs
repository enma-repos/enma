namespace Enma.Auth.Application.Dto;

public sealed record ExternalAuthRequestDto(
    string Provider,
    string Code,
    string? RedirectUri = null,
    string? CodeVerifier = null);