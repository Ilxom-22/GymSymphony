namespace Gymphony.Application.Common.Payments.Models.Settings;

public class StripeSettings
{
    public string PublicKey { get; set; } = default!;

    public string SecretKey { get; set; } = default!;
}