using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Payments.Commands;

public class UpdateStripeDetailsCommand : ICommand<StripeDetails>
{
    public StripeProductDetails UpdatedProductDetails { get; set; } = default!;

    public StripeDetails StripeDetails { get; set; } = default!;
}