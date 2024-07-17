using AutoMapper;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Events;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Common.Payments.Models.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IMediator mediator,
    IMapper mapper,
    IEventBusBroker eventBusBroker,
    IOptions<StripeSettings> stripeSettings) : ControllerBase
{
    private readonly StripeSettings _stripeSettings = stripeSettings.Value;
    
    [Authorize(Roles = "Member")]
    [HttpPost("customer-portal")]
    public async ValueTask<IActionResult> CreateCustomerPortalSession([FromBody] CreateBillingPortalSessionCommand createBillingPortalSessionCommand, 
        CancellationToken cancellationToken)
    {
        var sessionUrl = await mediator.Send(createBillingPortalSessionCommand, cancellationToken);

        return Ok(sessionUrl);
    }

    [HttpPost("webhook")]
    public async ValueTask<IActionResult> WebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, 
                Request.Headers["Stripe-Signature"], 
                _stripeSettings.WebHookSecret);

            if (stripeEvent.Type == Events.InvoicePaymentSucceeded)
            {
                var invoice = stripeEvent.Data.Object as Invoice;
                var subscription = mapper.Map<StripeInvoiceDto>(invoice);
                
                await eventBusBroker
                    .PublishLocalAsync(new StripeInvoicePaymentSucceededEvent { Invoice = subscription });
            }
            else if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var checkoutSession = stripeEvent.Data.Object as Session
                    ?? throw new InvalidOperationException();

                var stripeCheckoutSessionCompletedEvent = mapper.Map<StripeCheckoutSessionCompeletedEvent>(checkoutSession);

                await eventBusBroker.PublishLocalAsync(stripeCheckoutSessionCompletedEvent);
            }
            else if (stripeEvent.Type == Events.CheckoutSessionExpired || stripeEvent.Type == Events.CheckoutSessionAsyncPaymentFailed)
            {
                var checkoutSession = stripeEvent.Data.Object as Session
                    ?? throw new InvalidOperationException();

                var sessionId = checkoutSession.Id;

                await eventBusBroker.PublishLocalAsync(new StripeCheckoutSessionFailedEvent { SessionId = sessionId });
            }

            return Accepted();
        }
        catch (StripeException ex)
        {
            return BadRequest(ex.StripeError.Message);
        }
    }
}