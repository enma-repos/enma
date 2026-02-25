namespace Enma.Auth.Application.Dto.External;

public sealed record ExternalAuthRequestDto(
    string Provider,
    string Code,
    string? RedirectUri = null,
    string? CodeVerifier = null);