using System.Net.Http.Json;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Dto;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Options;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Http;

internal sealed class GoogleAuthClient
{
    private const string TokenEndpoint = "token";
    
    private readonly HttpClient _httpClient;
    private readonly ILogger<GoogleAuthClient> _logger;
    
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _redirectUrl;

    public GoogleAuthClient(
        HttpClient httpClient, 
        ILogger<GoogleAuthClient> logger,
        IOptions<GoogleProviderOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;

        _clientId = options.Value.ClientId;
        _clientSecret = options.Value.ClientSecret;
        _redirectUrl = options.Value.RedirectUrl;
    }

    public async Task<Result<GoogleTokenResponseDto>> GetUserTokenAsync(
        string code,
        CancellationToken ct)
    {
        using var form = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = _redirectUrl,
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret,
        });

        using var response = await _httpClient.PostAsync(TokenEndpoint, form, ct);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(ct);
            _logger.LogError("Google auth error. Response code: {responseCode}. Response body: {responseBody}",
                response.StatusCode, errorContent);
            
            return Result.Fail<GoogleTokenResponseDto>($"Google auth error. Response code: {response.StatusCode}");
        }

        var dto = await response.Content.ReadFromJsonAsync<GoogleTokenResponseDto>(cancellationToken: ct);
        return dto is null
            ? Result.Fail<GoogleTokenResponseDto>("Empty Google token response.")
            : Result.Ok(dto);
    }
}