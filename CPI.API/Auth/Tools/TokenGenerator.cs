using CPI.API.Auth.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CPI.API.Auth.Tools;

public class TokenGenerator (TokenConfiguration configurations)
{
    private readonly TokenConfiguration _configuration = configurations;

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.Minutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}
