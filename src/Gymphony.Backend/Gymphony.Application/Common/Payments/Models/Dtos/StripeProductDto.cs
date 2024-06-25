namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class StripeProductDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string? DefaultPrice { get; set; }

    public bool Active { get; set; } = true;
}