using Gymphony.Application.Products.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Products.Commands;

public class UpdateProductPriceCommand : ICommand<ProductDto>
{
    public Guid ProductId { get; set; }

    public decimal Price { get; set; }
}