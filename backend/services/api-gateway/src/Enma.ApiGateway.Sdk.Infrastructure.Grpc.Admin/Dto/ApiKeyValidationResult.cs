namespace Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Dto;

public sealed record ApiKeyValidationResult(
    Guid OrganizationId,
    Guid ProjectId,
    Guid SdkClientId,
    Guid ApiKeyId);
