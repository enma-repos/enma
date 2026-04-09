using Enma.Auth.Application.Models;
using Enma.Common.Enums;

namespace Enma.Auth.Application.Contracts.Infrastructure.Security;

public interface IAccessTokenProvider
{
    string GenerateToken(Account account, Guid sessionId, UserRole role);
}
