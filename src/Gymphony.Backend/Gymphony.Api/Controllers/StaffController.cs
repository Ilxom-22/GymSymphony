using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class StaffController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> GetAllStaff(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllStaffQuery());

        return Ok(result);
    }

    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> AddStaffAsync([FromForm] StaffSignUpCommand staffSignUpCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(staffSignUpCommand, cancellationToken);
        
        return Ok(result);
    }

    [HttpDelete("{staffId:guid}")]
    public async ValueTask<IActionResult> RemoveStaffAsync([FromRoute] Guid staffId, 
        CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveStaffCommand { StaffId= staffId }, cancellationToken);

        return NoContent();
    }
}