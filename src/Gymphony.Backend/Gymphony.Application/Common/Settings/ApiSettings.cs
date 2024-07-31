namespace Gymphony.Application.Common.Settings;

public class ApiSettings
{
    public string BaseAddress { get; set; } = default!;

    public string EmailVerificationUrl { get; set; } = default!;

    public string PasswordResetUrl { get; set; } = default!;
}