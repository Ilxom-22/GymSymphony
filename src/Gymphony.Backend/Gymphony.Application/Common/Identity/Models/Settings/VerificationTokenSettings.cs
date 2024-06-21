namespace Gymphony.Application.Common.Identity.Models.Settings;

public class VerificationTokenSettings
{
    public int EmailVerificationExpirationTimeInMinutes { get; set; }

    public int PasswordResetExpirationTimeInMinutes { get; set; }
}