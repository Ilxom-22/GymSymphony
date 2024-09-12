using Gymphony.Application.Courses.Queries;
using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Application.Subscriptions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[Authorize(Roles = "Member")]
[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController(IMediator mediator) : ControllerBase
{
    [HttpGet("my-membership-subscription")]
    public async ValueTask<IActionResult> GetMyMembershipPlan(CancellationToken cancellationToken)
    {
        var query = new GetMyMembershipPlanSubscriptionQuery();

        var result = await mediator.Send(query, cancellationToken);

        return result is null ? NoContent() : Ok(result);
    }

    [HttpGet("my-course-subscriptions")]
    public async ValueTask<IActionResult> GetMyCourseSubscriptions(CancellationToken cancellationToken)
    {
        var query = new GetMyCourseSubscriptionsQuery();

        var result = await mediator.Send(query, cancellationToken);

        return result.Count == 0 ? NoContent() : Ok(result);
    }

    [HttpGet("my-schedules")]
    public async ValueTask<IActionResult> GetMySchedules(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMySchedulesQuery());

        return Ok(result);
    }

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