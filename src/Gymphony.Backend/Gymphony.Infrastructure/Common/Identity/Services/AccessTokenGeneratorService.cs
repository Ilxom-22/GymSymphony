using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gymphony.Application.Common.Identity.Models.Settings;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Constants;
using Gymphony.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gymphony.Infrastructure.Common.Identity.Services;

public class AccessTokenGeneratorService(
    IOptions<JwtSettings> jwtSettings, 
    IOptions<JwtSecretKey> jwtSecretKey) : IAccessTokenGeneratorService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly JwtSecretKey _jwtSecretKey = jwtSecretKey.Value;
    
    public AccessToken GetAccessToken(User user)
    {
        var token = GenerateToken(user);

        var accessToken = new AccessToken
        {
            UserId = user.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes)
        };

        return accessToken;
    }
    
    private JwtSecurityToken GenerateToken(User user)
    {
        var claims = GetClaims(user);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
            signingCredentials: credentials
        );
        
        return token;
    }

    private List<Claim> GetClaims(User user)
        => 
        [
            new Claim(ClaimTypes.Email, user.EmailAddress),
            new Claim(ClaimConstants.UserId, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        ];
}