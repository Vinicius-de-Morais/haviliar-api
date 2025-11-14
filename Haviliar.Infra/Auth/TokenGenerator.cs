using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Haviliar.Domain.Auth.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Haviliar.Infra.Auth;

public class TokenGenerator(IOptions<JwtSettings> jwtSettings) : ITokenGenerator
{
    private readonly IOptions<JwtSettings> _jwtSettings = jwtSettings;

    public string GenerateAuthToken(int userId)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        return GenerateToken(claims, DateTime.UtcNow.AddDays(1));
    }

    private string GenerateToken(Claim[] claims, DateTime? expiresAt)
    {
        var signingCredentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key)),
        SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            expires: expiresAt);

        string? token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;

    }
}
