using System.Security.Cryptography;
using Gymphony.Application.Common.Identity.Models.Settings;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Gymphony.Infrastructure.Common.Identity.Services;

public class RefreshTokenGeneratorService(IOptions<RefreshTokenSettings> refreshTokenSettings) 
    : IRefreshTokenGeneratorService
{
    private readonly RefreshTokenSettings _refreshTokenSettings = refreshTokenSettings.Value;
    
    public RefreshToken GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = Convert.ToBase64String(randomNumber),
            ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(_refreshTokenSettings.ExpirationTimeInMinutes)
        };

        return refreshToken;
    }
}