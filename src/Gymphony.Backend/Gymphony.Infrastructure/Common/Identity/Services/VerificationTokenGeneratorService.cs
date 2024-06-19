using Gymphony.Application.Common.Identity.Models.Settings;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Gymphony.Infrastructure.Common.Identity.Services;

public class VerificationTokenGeneratorService(
    IOptions<VerificationTokenSettings> verificationTokenSettings,
    ITokenGeneratorService tokenGeneratorService) 
    : IVerificationTokenGeneratorService
{
    private readonly VerificationTokenSettings _verificationTokenSettings = verificationTokenSettings.Value;
    
    public VerificationToken GenerateVerificationToken(User user, VerificationType type)
    {
        var token = tokenGeneratorService.GenerateToken();

        var expiryTimeInMinutes = type == VerificationType.Email
            ? _verificationTokenSettings.EmailVerificationExpirationTimeInMinutes
            : _verificationTokenSettings.PasswordResetExpirationTimeInMinutes;

        var verificationToken = new VerificationToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            UserId = user.Id,
            Type = type,
            ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(expiryTimeInMinutes)
        };

        return verificationToken;
    }
}