using Gymphony.Application.Subscriptions.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Member")]
    [HttpPost("subscribe-for-membershipPlan")]
    public async ValueTask<IActionResult> SubscribeForMembershipPlan([FromBody] SubscribeForMembershipPlanCommand subscribeForMembershipPlanCommand, CancellationToken cancellationToken)
    {
        var checkoutSession = await mediator.Send(subscribeForMembershipPlanCommand, cancellationToken);

        return Ok(checkoutSession);
    }
}