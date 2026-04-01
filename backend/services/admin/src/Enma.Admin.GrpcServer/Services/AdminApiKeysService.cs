using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Grpc.Admin.ApiKeys.V1;
using Grpc.Core;

namespace Enma.Admin.GrpcServer.Services;

public sealed class AdminApiKeysService : Enma.Grpc.Admin.ApiKeys.V1.AdminApiKeysService.AdminApiKeysServiceBase
{
    private readonly IApiKeysRepository _apiKeysRepository;
    private readonly ISecretService _secretService;

    public AdminApiKeysService(
        IApiKeysRepository apiKeysRepository,
        ISecretService secretService)
    {
        _apiKeysRepository = apiKeysRepository;
        _secretService = secretService;
    }

    public override async Task<ValidateApiKeyResponse> ValidateApiKey(
        ValidateApiKeyRequest request,
        ServerCallContext context)
    {
        var plainKey = request.PlainKey;
        if (string.IsNullOrWhiteSpace(plainKey) || plainKey.Length < 6)
        {
            return new ValidateApiKeyResponse { Valid = false };
        }

        var prefix = plainKey.Length <= 12 ? plainKey : plainKey[..12];
        var hash = _secretService.ComputeHash(plainKey);

        var result = await _apiKeysRepository.FindActiveByHashAsync(hash, prefix, context.CancellationToken);
        if (result.IsFailed || result.Value is null)
        {
            return new ValidateApiKeyResponse { Valid = false };
        }

        var ctx = result.Value;
        return new ValidateApiKeyResponse
        {
            Valid = true,
            OrganizationId = ctx.OrganizationId.ToString(),
            ProjectId = ctx.ProjectId.ToString(),
            SdkClientId = ctx.SdkClientId.ToString(),
            ApiKeyId = ctx.ApiKeyId.ToString()
        };
    }
}
