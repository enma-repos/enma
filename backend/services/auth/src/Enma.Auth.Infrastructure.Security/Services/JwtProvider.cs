using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Enma.Auth.Application.Contracts.Infrastructure.Security;
using Enma.Auth.Application.Models;
using Enma.Common.Enums;
using Enma.Common.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Enma.Auth.Infrastructure.Security.Services;

internal sealed class JwtProvider : IAccessTokenProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(Account account, Guid sessionId, UserRole role)
    {
        Claim[] claims =
        {
            new("accountId", account.Id.ToString()),
            new("accountEmail", account.Email.Value),
            new("accountStatus", account.Status.ToString()),
            new("sessionId", sessionId.ToString()),
            // Write the short "role" claim — JwtBearer's DefaultInboundClaimTypeMap
            // will remap it to ClaimTypes.Role when the token is validated downstream.
            new("role", role.ToString())
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}
