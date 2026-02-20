using System.Security.Cryptography;
using System.Text;
using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.Other;

namespace Enma.Admin.Application.Services;

internal sealed class SecretService : ISecretService
{
    private const int MinKeyPrefixLength = 6;
    private const int MaxKeyPrefixLength = 32;
    
    private readonly byte[] _pepper;
    private readonly int _prefixLength;
    
    public SecretService(string pepper, int prefixLength = 12)
    {
        if (string.IsNullOrWhiteSpace(pepper))
        {
            throw new ArgumentException("Pepper is required.", nameof(pepper));
        }

        _pepper = Encoding.UTF8.GetBytes(pepper);
        _prefixLength = Math.Clamp(prefixLength, MinKeyPrefixLength, MaxKeyPrefixLength);
    }
    
    public KeyMaterial Generate(string tokenPrefix = "", int bytesCount = 32)
    {
        Span<byte> bytes = stackalloc byte[bytesCount];
        RandomNumberGenerator.Fill(bytes);
        
        var token = Base64UrlEncode(bytes);
        
        var plain = !string.IsNullOrWhiteSpace(tokenPrefix) ? $"{tokenPrefix}_{token}" : token;

        var prefix = plain.Length <= _prefixLength
            ? plain
            : plain.Substring(0, _prefixLength);

        var hash = ComputeHash(plain);

        return new KeyMaterial(plain, prefix, hash);
    }

    public string ComputeHash(string plainKey)
    {
        using var hmac = new HMACSHA256(_pepper);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(plainKey));
        
        return Convert.ToHexString(hashBytes);
    }
    
    private static string Base64UrlEncode(ReadOnlySpan<byte> data)
    {
        var s = Convert.ToBase64String(data);
        return s.TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}