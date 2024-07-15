using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Payments.Commands;

public class UpdateStripePriceCommand : ICommand<string>
{
    public StripeDetails StripeDetails { get; set; } = default!;
    
    public decimal NewPrice { get; set; } = default!;
}