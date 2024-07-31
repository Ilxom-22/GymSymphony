using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Common.Payments.Models.Settings;
using Gymphony.Domain.Common.Commands;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Gymphony.Infrastructure.Common.Payments.CommandHandlers;

public class CreateCheckoutSessionCommandHandler(IMediator mediator,
    IStripeCheckoutSessionBroker stripeCheckoutSessionBroker,
    IOptions<StripeSettings> stripeSettings)
    : ICommandHandler<CreateCheckoutSessionCommand, CheckoutSessionDto>
{
    private readonly StripeSettings _stripeSettings = stripeSettings.Value;
    
    public async Task<CheckoutSessionDto> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        var customerId = request.Member.StripeCustomerId ?? await mediator
            .Send(new CreateStripeCustomerIdCommand { Member = request.Member }, cancellationToken);
        
        var options = new SessionCreateOptions
        {
            SuccessUrl = request.SuccessUrl,
            CancelUrl = request.CancelUrl,
            PaymentMethodTypes = ["card"],
            Mode = "subscription",
            Customer = customerId,
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = request.PriceId,
                    Quantity = 1
                }
            ]
        };

        try
        {
            var session = await stripeCheckoutSessionBroker.CreateAsync(options, cancellationToken: cancellationToken);
            return new CheckoutSessionDto { SessionId = session.Id, PublicKey = _stripeSettings.PublicKey };
        }
        catch (StripeException ex)
        {
            throw new InvalidOperationException(ex.StripeError.Message);
        }
    }
}