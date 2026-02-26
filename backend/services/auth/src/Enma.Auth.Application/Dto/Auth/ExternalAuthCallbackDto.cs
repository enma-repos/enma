namespace Enma.Auth.Application.Dto.Auth;

public sealed record ExternalAuthCallbackDto(
    string Code, 
    string State);