using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Abstractions;
using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Dto;
using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Options;
using Enma.Grpc.Admin.ApiKeys.V1;
using FluentResults;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Clients;

internal sealed class AdminApiKeysClient : IAdminApiKeysClient
{
    private readonly AdminApiKeysService.AdminApiKeysServiceClient _client;
    private readonly AdminGrpcOptions _options;

    public AdminApiKeysClient(
        AdminApiKeysService.AdminApiKeysServiceClient client,
        IOptions<AdminGrpcOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<Result<ApiKeyValidationResult>> ValidateAsync(string plainKey, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.ValidateApiKeyAsync(
                    new ValidateApiKeyRequest { PlainKey = plainKey },
                    deadline: DateTime.UtcNow.AddMilliseconds(_options.DeadlineMs),
                    cancellationToken: ct)
                .ResponseAsync;

            if (!response.Valid)
            {
                return Result.Fail<ApiKeyValidationResult>("Invalid API key.");
            }

            if (!Guid.TryParse(response.OrganizationId, out var orgId) ||
                !Guid.TryParse(response.ProjectId, out var projectId) ||
                !Guid.TryParse(response.SdkClientId, out var sdkClientId) ||
                !Guid.TryParse(response.ApiKeyId, out var apiKeyId))
            {
                return Result.Fail<ApiKeyValidationResult>("Invalid response from Admin service.");
            }

            return Result.Ok(new ApiKeyValidationResult(orgId, projectId, sdkClientId, apiKeyId));
        }
        catch (RpcException ex)
        {
            return Result.Fail<ApiKeyValidationResult>($"gRPC error: {ex.Status.Detail}");
        }
    }
}
