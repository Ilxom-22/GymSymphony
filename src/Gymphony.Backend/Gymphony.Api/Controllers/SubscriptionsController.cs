using Gymphony.Application.Subscriptions.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[Authorize(Roles = "Member")]
[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController(IMediator mediator) : ControllerBase
{
    [HttpPost("subscribe-for-membershipPlan")]
    public async ValueTask<IActionResult> SubscribeForMembershipPlan([FromBody] SubscribeForMembershipPlanCommand subscribeForMembershipPlanCommand, CancellationToken cancellationToken)
    {
        var checkoutSession = await mediator.Send(subscribeForMembershipPlanCommand, cancellationToken);

        return Ok(checkoutSession);
    }

    [HttpPost("subscribe-for-course")]
    public async ValueTask<IActionResult> SubscribeForCourse([FromBody] SubscribeForCourseCommand command, CancellationToken cancellationToken)
    {
        var checkoutSession = await mediator.Send(command, cancellationToken);

        return Ok(checkoutSession);
    }
}