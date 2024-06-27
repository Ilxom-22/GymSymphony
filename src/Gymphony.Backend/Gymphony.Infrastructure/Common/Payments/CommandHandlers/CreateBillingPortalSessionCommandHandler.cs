using System.Security.Authentication;
using FluentValidation;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Infrastructure.Common.Payments.Validators;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.BillingPortal;

namespace Gymphony.Infrastructure.Common.Payments.CommandHandlers;

public class CreateBillingPortalSessionCommandHandler(
    IRequestContextProvider requestContextProvider,
    IMemberRepository memberRepository,
    IStripeBillingPortalSessionBroker stripeBillingPortalSessionBroker,
    IValidator<CreateBillingPortalSessionCommand> createBillingPortalSessionCommandValidator)
    : ICommandHandler<CreateBillingPortalSessionCommand, string>
{
    public async Task<string> Handle(CreateBillingPortalSessionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await createBillingPortalSessionCommandValidator
            .ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors[0].ToString());
        
        var memberId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized Access!");

        var stripeCustomerId = await memberRepository
            .Get(member => member.Id == memberId)
            .Select(member => member.StripeCustomerId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Customer Billing Portal for Member with id {memberId} does not exist!");
        
        var options = new SessionCreateOptions
        {
            Customer = stripeCustomerId,
            ReturnUrl = request.ReturnUrl
        };

        try
        {
            var session = await stripeBillingPortalSessionBroker.CreateAsync(options, cancellationToken);
            return session.Url;
        }
        catch (StripeException ex)
        {
            throw new InvalidOperationException(ex.StripeError.Message);
        }
    }
}