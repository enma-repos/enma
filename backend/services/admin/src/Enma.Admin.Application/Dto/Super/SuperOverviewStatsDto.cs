namespace Enma.Admin.Application.Dto.Super;

public sealed record SuperOverviewStatsDto(
    int TotalUsers,
    int TotalOrganizations,
    int TotalProjects,
    int TotalApiKeys,
    int RecentAuditLogsLast24Hours);
