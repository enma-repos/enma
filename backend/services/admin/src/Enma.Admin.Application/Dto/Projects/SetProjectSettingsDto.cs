using System.Text.Json.Nodes;

namespace Enma.Admin.Application.Dto.Projects;

public sealed record SetProjectSettingsDto(JsonObject? Settings);

