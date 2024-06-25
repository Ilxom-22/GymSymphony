namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class StripePriceDto
{
    public string Id { get; set; } = default!;
    
    public string Currency { get; set; } = "usd";
    
    public decimal UnitAmountDecimal { get; set; }

    public bool Active { get; set; } = true;
    
    public string Product { get; set; } = default!;

    public StripePriceRecurringDto? Recurring { get; set; }
}