using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class StripeProductDetails
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; } = default!;

    public DurationUnit DurationUnit { get; set; }

    public byte DurationCount { get; set; }

    public ProductType Type { get; set; }
}