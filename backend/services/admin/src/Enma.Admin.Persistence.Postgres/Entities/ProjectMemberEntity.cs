using Enma.Common.Enums;

namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class ProjectMemberEntity
{
    public Guid ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }

    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    public ProjectRole Role { get; set; }

    public DateTime JoinedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}