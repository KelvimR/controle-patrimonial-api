using CPI.API.CPI.Application.DTOs;
using CPI.API.CPI.Domain.Entities;
using System.Security.Claims;

namespace CPI.API.Auth.Contracts;

public interface ITokenGenerator
{
    string GenerateAccessToken(IEnumerable<Claim>);
    string GenerateRefreshToken();
    TokenDTO GenerateToken(User user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

}
