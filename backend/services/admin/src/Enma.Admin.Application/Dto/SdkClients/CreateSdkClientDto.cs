using System.Text.Json.Nodes;
using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.SdkClients;

public sealed record CreateSdkClientDto(
    Guid ProjectId,
    string? Name,
    string? Description,
    SdkClientType Type,
    JsonObject? Settings);

