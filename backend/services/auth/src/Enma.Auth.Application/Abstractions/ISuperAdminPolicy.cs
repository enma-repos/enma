namespace Enma.Auth.Application.Abstractions;

public interface ISuperAdminPolicy
{
    bool IsSuperAdmin(string email);
}
