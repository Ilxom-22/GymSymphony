namespace Gymphony.Application.Products.Models.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Status { get; set; } = default!;
    
    public string DurationUnit { get; set; } = default!;

    public int DurationCount { get; set; }
    
    public decimal Price { get; set; }

    public string Type { get; set; } = default!;

    public DateOnly? ActivationDate { get; set; }
}