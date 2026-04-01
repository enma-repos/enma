namespace Enma.Admin.Application.Dto.ApiKeys;

public sealed record ApiKeyWithContextDto(
    Guid ApiKeyId,
    Guid SdkClientId,
    Guid ProjectId,
    Guid OrganizationId);
