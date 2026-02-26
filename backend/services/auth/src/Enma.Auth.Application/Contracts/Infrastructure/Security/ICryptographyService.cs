namespace Enma.Auth.Application.Contracts.Infrastructure.Security;

public interface ICryptographyService
{
    string ComputeSha256Hex(string value);
    string GenerateToken(int bytesCount = 32);
}