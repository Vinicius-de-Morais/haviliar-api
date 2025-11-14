using System.Text;
using System.Text.Json;
using Haviliar.Domain.Auth.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Haviliar.Infra.Auth;

public class TokenService(IOptions<JwtSettings> jwtOptions) : ITokenService
{
    private readonly IOptions<JwtSettings> _jwtOptions = jwtOptions;
    public async Task<T?> ExtractData<T>(string token)
    {
        var handler = new JsonWebTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = _jwtOptions.Value.Audience,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Value.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key)),
        };
        TokenValidationResult? validationResult = await handler.ValidateTokenAsync(token, validationParameters);

        if (!validationResult.IsValid)
        {
            return default;
        }

        try
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync<IDictionary<string, object>>(stream, validationResult.Claims);

            var json = Encoding.UTF8.GetString(stream.ToArray());

            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception)
        {
            return default;
        }
    }
}
