using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class StripeProductDto
{
    public string Id { get; set; } = default!;

    public string Name { get; init; } = default!;

    public string Description { get; init; } = default!;

    public string? DefaultPrice { get; init; }

    public bool Active { get; init; } = true;
    
    public ProductType ProductType { get; init; }
}