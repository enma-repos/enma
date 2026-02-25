using Enma.Auth.Application.Models;

namespace Enma.Auth.Application.Contracts.Infrastructure.Security;

public interface IAccessTokenProvider
{
    string GenerateToken(Account account, Guid sessionId);
}