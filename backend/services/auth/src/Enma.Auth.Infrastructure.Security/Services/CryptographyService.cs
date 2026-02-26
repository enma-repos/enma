using System.Security.Cryptography;
using System.Text;
using Enma.Auth.Application.Contracts.Infrastructure.Security;

namespace Enma.Auth.Infrastructure.Security.Services;

internal sealed class CryptographyService : ICryptographyService
{
    public string GenerateToken(int bytesCount = 32)
    {
        var bytes = RandomNumberGenerator.GetBytes(bytesCount);
        return Base64UrlEncode(bytes);
    }

    public string ComputeSha256Hex(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
    
    private string Base64UrlEncode(byte[] bytes)
    {
        return Convert
            .ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}