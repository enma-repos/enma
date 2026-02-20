using System.Text.Json.Nodes;

namespace Enma.Admin.Application.Dto.SdkClients;

public sealed record SetSdkClientSettingsDto(JsonObject? Settings);

