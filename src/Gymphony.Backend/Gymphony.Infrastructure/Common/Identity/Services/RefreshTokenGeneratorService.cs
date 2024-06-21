using Gymphony.Application.Common.Identity.Models.Settings;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Gymphony.Infrastructure.Common.Identity.Services;

public class RefreshTokenGeneratorService(
    IOptions<RefreshTokenSettings> refreshTokenSettings,
    ITokenGeneratorService tokenGeneratorService) 
    : IRefreshTokenGeneratorService
{
    private readonly RefreshTokenSettings _refreshTokenSettings = refreshTokenSettings.Value;
    
    public RefreshToken GenerateRefreshToken(User user)
    {
        var token = tokenGeneratorService.GenerateToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = token,
            ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(_refreshTokenSettings.ExpirationTimeInMinutes)
        };

        return refreshToken;
    }
}