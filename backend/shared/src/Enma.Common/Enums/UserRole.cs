using System.Text.Json.Serialization;

namespace Enma.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<UserRole>))]
public enum UserRole
{
    Member = 0,
    SuperAdmin = 1
}
