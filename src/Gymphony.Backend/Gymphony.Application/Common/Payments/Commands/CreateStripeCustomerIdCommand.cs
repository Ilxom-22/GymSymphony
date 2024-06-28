using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Payments.Commands;

public class CreateStripeCustomerIdCommand : ICommand<string>
{
    public Member Member { get; set; } = default!;
}