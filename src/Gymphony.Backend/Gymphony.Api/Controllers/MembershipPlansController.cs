using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Application.MembershipPlans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipPlansController(IMediator mediator) : ControllerBase
{
    [HttpGet("public")]
    public async ValueTask<IActionResult> GetPublicMembershipPlans(CancellationToken cancellationToken)
    {
        var queryResult = await mediator
            .Send(new GetPublicMembershipPlansQuery(), cancellationToken);
        
        return Ok(queryResult);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllMembershipPlans(CancellationToken cancellationToken)
    {
        var queryResult = await mediator
            .Send(new GetAllMembershipPlansQuery(), cancellationToken);
        
        return Ok(queryResult);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("plan-details/{membershipPlanId:guid}")]
    public async ValueTask<IActionResult> GetMembershipPlanDetails(
        [FromRoute] Guid membershipPlanId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMembershipPlanDetailsQuery 
            { MembershipPlanId = membershipPlanId }, cancellationToken);
        
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async ValueTask<IActionResult> CreateMembershipPlan([FromBody] DraftMembershipPlanDto draftMembershipPlanDto, CancellationToken cancellationToken)
    {
        var createMembershipPlanCommand = new CreateMembershipPlanCommand 
            { MembershipPlan = draftMembershipPlanDto };
        
        var result = await mediator
            .Send(createMembershipPlanCommand, cancellationToken);
        
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async ValueTask<IActionResult> UpdateDraftMembershipPlan([FromBody] UpdateDraftMembershipPlanCommand updateDraftMembershipPlanCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(updateDraftMembershipPlanCommand, cancellationToken);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("publish")]
    public async ValueTask<IActionResult> PublishMembershipPlan([FromBody] PublishMembershipPlanCommand publishMembershipPlanCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(publishMembershipPlanCommand, cancellationToken);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{membershipPlanId:guid}")]
    public async ValueTask<IActionResult> DeleteDraftMembershipPlan([FromRoute] Guid membershipPlanId, CancellationToken cancellationToken)
    {
        var removeDraftMembershipPlanCommand = new RemoveDraftMembershipPlanCommand
            { MembershipPlanId = membershipPlanId };
        
        await mediator.Send(removeDraftMembershipPlanCommand, cancellationToken);

        return NoContent();
    }
}