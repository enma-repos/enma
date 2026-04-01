using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Dto;
using FluentResults;

namespace Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Abstractions;

public interface IAdminApiKeysClient
{
    Task<Result<ApiKeyValidationResult>> ValidateAsync(string plainKey, CancellationToken ct = default);
}
