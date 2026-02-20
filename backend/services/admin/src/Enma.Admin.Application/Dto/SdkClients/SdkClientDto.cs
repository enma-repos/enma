using System.Text.Json.Nodes;
using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.SdkClients;

public sealed record SdkClientDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    SdkClientType Type,
    string? Description,
    JsonObject? Settings,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt);

