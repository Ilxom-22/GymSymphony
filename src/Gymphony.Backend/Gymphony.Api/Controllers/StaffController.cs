using Gymphony.Application.Common.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> AddStaffAsync([FromBody] StaffSignUpCommand staffSignUpCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(staffSignUpCommand, cancellationToken);
        
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{staffId:guid}")]
    public async ValueTask<IActionResult> RemoveStaffAsync([FromRoute] Guid staffId, 
        CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveStaffCommand { StaffId= staffId }, cancellationToken);

        return NoContent();
    }
}