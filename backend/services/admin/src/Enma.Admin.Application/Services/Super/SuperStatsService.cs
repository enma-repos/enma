using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Super;
using FluentResults;

namespace Enma.Admin.Application.Services.Super;

internal sealed class SuperStatsService : ISuperStatsService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IProjectsRepository _projectsRepository;
    private readonly IApiKeysRepository _apiKeysRepository;
    private readonly IAuditLogsRepository _auditLogsRepository;

    public SuperStatsService(
        IUsersRepository usersRepository,
        IOrganizationsRepository organizationsRepository,
        IProjectsRepository projectsRepository,
        IApiKeysRepository apiKeysRepository,
        IAuditLogsRepository auditLogsRepository)
    {
        _usersRepository = usersRepository;
        _organizationsRepository = organizationsRepository;
        _projectsRepository = projectsRepository;
        _apiKeysRepository = apiKeysRepository;
        _auditLogsRepository = auditLogsRepository;
    }

    public async Task<Result<SuperOverviewStatsDto>> GetOverviewAsync(CancellationToken ct = default)
    {
        var usersRes = await _usersRepository.CountAllAsync(includeDeleted: false, ct);
        if (usersRes.IsFailed) return Result.Fail<SuperOverviewStatsDto>(usersRes.Errors);

        var orgsRes = await _organizationsRepository.CountAllAsync(includeDeleted: false, ct);
        if (orgsRes.IsFailed) return Result.Fail<SuperOverviewStatsDto>(orgsRes.Errors);

        var projectsRes = await _projectsRepository.CountAllAsync(includeDeleted: false, ct);
        if (projectsRes.IsFailed) return Result.Fail<SuperOverviewStatsDto>(projectsRes.Errors);

        var apiKeysRes = await _apiKeysRepository.CountAllAsync(ct);
        if (apiKeysRes.IsFailed) return Result.Fail<SuperOverviewStatsDto>(apiKeysRes.Errors);

        var since = DateTime.UtcNow.AddHours(-24);
        var recentAuditRes = await _auditLogsRepository.CountRecentAllAsync(since, ct);
        if (recentAuditRes.IsFailed) return Result.Fail<SuperOverviewStatsDto>(recentAuditRes.Errors);

        return Result.Ok(new SuperOverviewStatsDto(
            TotalUsers: usersRes.Value,
            TotalOrganizations: orgsRes.Value,
            TotalProjects: projectsRes.Value,
            TotalApiKeys: apiKeysRes.Value,
            RecentAuditLogsLast24Hours: recentAuditRes.Value));
    }
}
