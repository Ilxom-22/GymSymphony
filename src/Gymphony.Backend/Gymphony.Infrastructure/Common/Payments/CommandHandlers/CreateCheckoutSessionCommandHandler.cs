using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Common.Payments.Models.Settings;
using Gymphony.Application.Common.Payments.Queries;
using Gymphony.Domain.Common.Commands;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Gymphony.Infrastructure.Common.Payments.CommandHandlers;

public class CreateCheckoutSessionCommandHandler(IMediator mediator, IMapper mapper, 
    IStripeSessionBroker stripeSessionBroker,
    IValidator<CreateCheckoutSessionCommand> createCheckoutSessionCommandValidator,
    IOptions<StripeSettings> stripeSettings)
    : ICommandHandler<CreateCheckoutSessionCommand, CheckoutSessionDto>
{
    private readonly StripeSettings _stripeSettings = stripeSettings.Value;
    
    public async Task<CheckoutSessionDto> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await createCheckoutSessionCommandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors[0].ToString());
        
        var getStripePriceIdQuery = mapper.Map<GetStripePriceIdQuery>(request);
        
        var priceId = await mediator.Send(getStripePriceIdQuery, cancellationToken)
            ?? throw new ArgumentException("Invalid Product Id!");
        
        var options = new SessionCreateOptions
        {
            SuccessUrl = request.SuccessUrl,
            CancelUrl = request.CancelUrl,
            PaymentMethodTypes = ["card"],
            Mode = "subscription",
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1
                }
            ]
        };

        try
        {
            var session = await stripeSessionBroker.CreateAsync(options, cancellationToken: cancellationToken);
            return new CheckoutSessionDto { SessionId = session.Id, PublicKey = _stripeSettings.PublicKey };
        }
        catch (StripeException ex)
        {
            throw new InvalidOperationException(ex.StripeError.Message);
        }
    }
}