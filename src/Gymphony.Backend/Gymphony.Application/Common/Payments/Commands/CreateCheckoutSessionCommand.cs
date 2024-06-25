using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Payments.Commands;

public class CreateCheckoutSessionCommand : ICommand<CheckoutSessionDto>
{
    public Guid ProductId { get; set; }

    public string ProductType { get; set; } = default!;

    public string SuccessUrl { get; set; } = default!;

    public string CancelUrl { get; set; } = default!;
}