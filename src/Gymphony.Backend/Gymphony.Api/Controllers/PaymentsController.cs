using Gymphony.Application.Common.Payments.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    [HttpPost("create-checkout-session")]
    public async ValueTask<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionCommand createCheckoutSessionCommand, CancellationToken cancellationToken)
    {
        var sessionId = await mediator.Send(createCheckoutSessionCommand, cancellationToken);

        return Ok(sessionId);
    }
}