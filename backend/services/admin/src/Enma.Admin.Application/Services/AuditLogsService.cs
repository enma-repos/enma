using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;

namespace Enma.Admin.Application.Services;

internal sealed class AuditLogsService : IAuditLogsService
{
    private readonly IAuditLogsRepository _auditLogsRepository;

    public AuditLogsService(IAuditLogsRepository auditLogsRepository)
    {
        _auditLogsRepository = auditLogsRepository;
    }
}