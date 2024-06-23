using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleAuthenticationAuthorization.Options;

namespace SimpleAuthenticationAuthorization.Services;

public class IdentityService(IOptions<JwtSettings> jwtSettings)
{
    private readonly IOptions<JwtSettings> jwtSettings = jwtSettings;
    private readonly byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Value.SigningKey);

    private static JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

    public SecurityToken GenerateJwtToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);
        return jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
    }
    
    public string GenerateJwtTokenString(SecurityToken token)
    {
        return jwtSecurityTokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
    {
        return new SecurityTokenDescriptor()
        {
            Subject = identity,
            Issuer = jwtSettings.Value.Issuer,
            Audience = jwtSettings.Value.Audience[0],
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
    }
}