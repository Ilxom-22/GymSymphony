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
    [HttpPost("create-checkout-session")]
    public async ValueTask<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionCommand createCheckoutSessionCommand, CancellationToken cancellationToken)
    {
        var sessionId = await mediator.Send(createCheckoutSessionCommand, cancellationToken);

        return Ok(sessionId);
    }
    
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

            switch (stripeEvent.Type)
            {
                case Events.CheckoutSessionCompleted:
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    var subscription = mapper.Map<StripeSubscriptionDto>(session);
                    
                    await eventBusBroker
                        .PublishLocalAsync(new StripeCheckoutSessionCompletedEvent { Subscription = subscription });
                    
                    break;
                }
                case Events.InvoicePaymentSucceeded:
                {
                    var invoice = stripeEvent.Data.Object as Invoice;
                    var subscription = mapper.Map<StripeSubscriptionDto>(invoice);
                    
                    await eventBusBroker
                        .PublishLocalAsync(new StripeInvoicePaymentSucceededEvent { Subscription = subscription });
                    
                    break;
                }
            }

            return Accepted();
        }
        catch (StripeException ex)
        {
            return BadRequest(ex.StripeError.Message);
        }
    }
}