namespace Enma.Auth.Application.Dto.Cache;

public sealed record CachedStateDto(
    string Provider,
    string? SuccessUrl);