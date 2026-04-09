using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Options;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Application.Services;

internal sealed class SuperAdminPolicy : ISuperAdminPolicy
{
    private readonly HashSet<string> _emails;

    public SuperAdminPolicy(IOptions<SuperAdminOptions> options)
    {
        var raw = options.Value.EmailsRaw ?? string.Empty;
        _emails = raw
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public bool IsSuperAdmin(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        return _emails.Contains(email.Trim());
    }
}
