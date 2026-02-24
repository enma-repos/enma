using Enma.Auth.Infrastructure.ExternalAuth.Google.Abstractions;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Dto;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Options;
using FluentResults;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Services;

internal sealed class GoogleIdTokenValidator : IGoogleIdTokenValidator
{
    private readonly GoogleProviderOptions _options;
    private readonly ILogger<GoogleIdTokenValidator> _logger;
    
    public GoogleIdTokenValidator(
        IOptions<GoogleProviderOptions> options,
        ILogger<GoogleIdTokenValidator> logger)
    {
        _options = options.Value;
        _logger = logger;
    }
    
    public async Task<Result<GoogleIdTokenClaimsDto>> ValidateAsync(string idToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            return Result.Fail<GoogleIdTokenClaimsDto>("Empty id_token.");
        }

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            
            if (string.IsNullOrWhiteSpace(payload.Subject))
            {
                return Result.Fail<GoogleIdTokenClaimsDto>("Missing subject.");
            }

            if (_options.RequireEmailVerified && !payload.EmailVerified)
            {
                return Result.Fail<GoogleIdTokenClaimsDto>("Google email is not verified.");
            }

            return Result.Ok(new GoogleIdTokenClaimsDto(
                Subject: payload.Subject,
                Email: payload.Email,
                EmailVerified: payload.EmailVerified,
                Name: payload.Name,
                PictureUrl: payload.Picture));
        }
        catch (InvalidJwtException ex)
        {
            _logger.LogInformation(ex, "Invalid Google id_token.");
            return Result.Fail<GoogleIdTokenClaimsDto>("Invalid Google id_token.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Unexpected error validating Google id_token.");
            return Result.Fail<GoogleIdTokenClaimsDto>("Failed to validate Google id_token.");
        }
    }
}