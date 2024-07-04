using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Payments.Commands;

public class CreateCheckoutSessionCommand : ICommand<CheckoutSessionDto>
{
    public string PriceId { get; set; } = default!;

    public Member Member { get; set; } = default!;

    public string SuccessUrl { get; set; } = default!;

    public string CancelUrl { get; set; } = default!;
}