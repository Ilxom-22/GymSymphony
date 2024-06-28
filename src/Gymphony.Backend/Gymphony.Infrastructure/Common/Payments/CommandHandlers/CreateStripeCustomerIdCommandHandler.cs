using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Persistence.Repositories.Interfaces;
using Stripe;

namespace Gymphony.Infrastructure.Common.Payments.CommandHandlers;

public class CreateStripeCustomerIdCommandHandler(
    IMemberRepository memberRepository,
    IStripeCustomerBroker stripeCustomerBroker) 
    : ICommandHandler<CreateStripeCustomerIdCommand, string>
{
    public async Task<string> Handle(CreateStripeCustomerIdCommand request, CancellationToken cancellationToken)
    {
        var customerCreateOptions = new CustomerCreateOptions
        {
            Name = $"{request.Member.FirstName} {request.Member.LastName}",
            Email = request.Member.EmailAddress
        };

        var customer = await stripeCustomerBroker.CreateAsync(customerCreateOptions, cancellationToken);
        request.Member.StripeCustomerId = customer.Id;
        
        await memberRepository.UpdateAsync(request.Member, cancellationToken: cancellationToken);

        return customer.Id;
    }
}