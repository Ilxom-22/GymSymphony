using System.Security.Authentication;
using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Common.Payments.Models.Settings;
using Gymphony.Application.Common.Payments.Queries;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Gymphony.Infrastructure.Common.Payments.CommandHandlers;

public class CreateCheckoutSessionCommandHandler(IMediator mediator, IMapper mapper, 
    IStripeCheckoutSessionBroker stripeCheckoutSessionBroker,
    IValidator<CreateCheckoutSessionCommand> createCheckoutSessionCommandValidator,
    IMemberRepository memberRepository,
    IRequestContextProvider requestContextProvider,
    IOptions<StripeSettings> stripeSettings)
    : ICommandHandler<CreateCheckoutSessionCommand, CheckoutSessionDto>
{
    private readonly StripeSettings _stripeSettings = stripeSettings.Value;
    
    public async Task<CheckoutSessionDto> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await createCheckoutSessionCommandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors[0].ToString());
        
        var memberId = requestContextProvider.GetUserIdFromClaims()
                       ?? throw new AuthenticationException("Unauthorized access!");

        var member = await memberRepository
            .Get(member => member.Id == memberId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AuthenticationException("Unauthorized access!");

        var customerId = member.StripeCustomerId ?? await mediator
            .Send(new CreateStripeCustomerIdCommand { Member = member }, cancellationToken);
        
        var getStripePriceIdQuery = mapper.Map<GetStripePriceIdQuery>(request);
        
        var priceId = await mediator.Send(getStripePriceIdQuery, cancellationToken)
            ?? throw new ArgumentException("Invalid Product Id!");
        
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
                    Price = priceId,
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