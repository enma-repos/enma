using Enma.Admin.Application.Dto.Super;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface ISuperStatsService
{
    Task<Result<SuperOverviewStatsDto>> GetOverviewAsync(CancellationToken ct = default);
}
