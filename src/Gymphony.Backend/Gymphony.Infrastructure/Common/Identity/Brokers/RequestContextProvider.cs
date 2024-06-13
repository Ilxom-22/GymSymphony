using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Gymphony.Application.Common.Identity.Models.Settings;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gymphony.Infrastructure.Common.Identity.Brokers;

public class RequestContextProvider(
    IHttpContextAccessor httpContextAccessor,
    IOptions<JwtSettings> jwtSettings,
    IOptions<JwtSecretKey> jwtSecretKey)
    : IRequestContextProvider
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly JwtSecretKey _jwtSecretKey = jwtSecretKey.Value;
    
    public Guid? GetUserIdFromClaims()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var userIdClaim = httpContext!.User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.UserId)?.Value;

        return userIdClaim is not null ? Guid.Parse(userIdClaim) : null;
    }

    public Guid? GetUserIdFromClaimsOrToken()
    {
        var claimsUserId = GetUserIdFromClaims();

        if (claimsUserId is not null)
            return (Guid)claimsUserId;

        var accessToken = GetAccessToken();

        if (string.IsNullOrWhiteSpace(accessToken)) return null;
        
        var jwtHandler = new JwtSecurityTokenHandler();
        if (!jwtHandler.CanReadToken(accessToken)) return null;
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidIssuer = _jwtSettings.ValidIssuer,
            ValidateAudience = _jwtSettings.ValidateAudience,
            ValidAudience = _jwtSettings.ValidAudience,
            ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey.SecretKey)),
            ValidateLifetime = false
        };

        _ = jwtHandler.ValidateToken(accessToken, tokenValidationParameters,
            out var validatedToken);
        var jwtToken = validatedToken as JwtSecurityToken;
        var userId = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.UserId)?.Value;

        return userId is not null ? Guid.Parse(userId) : null;
    }

    public string? GetAccessToken()
    {
        var accessTokenValue = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

        if (!string.IsNullOrEmpty(accessTokenValue) && accessTokenValue.StartsWith("Bearer "))
            return accessTokenValue["Bearer ".Length..].Trim();
        
        return null;
    }
}