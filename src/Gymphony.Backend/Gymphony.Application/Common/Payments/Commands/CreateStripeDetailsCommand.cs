using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Payments.Commands;

public class CreateStripeDetailsCommand : ICommand<StripeDetails>
{
    public StripeProductDetails ProductDetails { get; set; } = default!;
}