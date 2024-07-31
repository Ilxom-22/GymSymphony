namespace Gymphony.Domain.Entities;

public class StripeDetails
{
    private Guid Id { get; set; }
    
    public string ProductId { get; set; } = default!;

    public string PriceId { get; set; } = default!;
}