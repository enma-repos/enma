using Enma.Auth.Application.Contracts.Infrastructure.ExternalAuth;
using Enma.Auth.Application.Dto;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Abstractions;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Http;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Services;

internal sealed class GoogleExternalIdentityProvider : IExternalIdentityProvider
{
    public string Name => "google";
    
    private readonly GoogleAuthClient _googleAuthClient;
    private readonly IGoogleIdTokenValidator _idTokenValidator;
    private readonly ILogger<GoogleExternalIdentityProvider> _logger;
    
    public GoogleExternalIdentityProvider(
        GoogleAuthClient googleAuthClient, 
        IGoogleIdTokenValidator idTokenValidator,
        ILogger<GoogleExternalIdentityProvider> logger)
    {
        _googleAuthClient = googleAuthClient;
        _idTokenValidator = idTokenValidator;
        _logger = logger;
    }
    
    public async Task<Result<ExternalIdentityDto>> AuthenticateAsync(
        ExternalAuthRequestDto request, 
        CancellationToken ct)
    {
        var tokenResult = await _googleAuthClient.GetUserTokenAsync(request.Code, ct);
        if (tokenResult.IsFailed)
        {
            return Result.Fail<ExternalIdentityDto>(tokenResult.Errors);
        }

        var token = tokenResult.Value;
        if (string.IsNullOrWhiteSpace(token.IdToken))
        {
            return Result.Fail<ExternalIdentityDto>("Google token response missing id_token.");
        }
        
        var validationResult = await _idTokenValidator.ValidateAsync(token.IdToken!, ct);
        if (validationResult.IsFailed)
        {
            return Result.Fail<ExternalIdentityDto>(validationResult.Errors);
        }
        
        var identity = new ExternalIdentityDto(
            Provider: Name,
            Subject: validationResult.Value.Subject,
            Email: validationResult.Value.Email,
            EmailVerified: validationResult.Value.EmailVerified,
            DisplayName: validationResult.Value.Name,
            PictureUrl: validationResult.Value.PictureUrl
        );

        return Result.Ok(identity);
    }
}