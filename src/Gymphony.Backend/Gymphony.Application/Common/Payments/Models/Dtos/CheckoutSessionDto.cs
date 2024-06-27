namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class CheckoutSessionDto
{
    public string SessionId { get; set; } = default!;

    public string PublicKey { get; set; } = default!;
}