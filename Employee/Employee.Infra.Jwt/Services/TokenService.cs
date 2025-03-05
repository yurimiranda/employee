using Employee.Domain.Models;
using Employee.Domain.Services;
using Employee.Infra.Jwt.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Employee.Infra.Jwt.Services;

public class TokenService(
    IOptions<JwtConfiguration> options,
    IJwtService jwtService) : ITokenService
{
    private readonly JwtConfiguration _jwtConfiguration = options.Value;

    public async Task<string> GenerateToken(UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var now = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            IssuedAt = now,
            NotBefore = now,
            Subject = new ClaimsIdentity(
            [
                    new(ClaimTypes.Name, user.Employee.Name),
                    new(ClaimTypes.GivenName, user.Employee.Name),
                    new(ClaimTypes.Role, user.Role.ToString() ?? "None"),
                    new(ClaimTypes.Email, user.Username, ClaimValueTypes.Email),
                    new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new("user_id", user.Id.ToString())
            ]),
            Expires = now.AddHours(_jwtConfiguration.ExpiresInHrs),
            SigningCredentials = await jwtService.GetCurrentSigningCredentials()
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}