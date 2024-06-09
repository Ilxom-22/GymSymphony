namespace Gymphony.Application.Common.Identity.Models.Settings;

public class JwtSettings
{
    public string ValidIssuer { get; set; } = default!;

    public string ValidAudience { get; set; } = default!;

    public bool ValidateIssuer { get; set; } = default!;

    public bool ValidateAudience { get; set; } = default!;

    public bool ValidateLifetime { get; set; }
    
    public bool ValidateIssuerSigningKey { get; set; }
    
    public int ExpirationTimeInMinutes { get; set; }

    public int RefreshTokenExpirationTimeInMinutes { get; set; }

    public int RefreshTokenExtendedExpirationTimeInMinutes { get; set; }
}
