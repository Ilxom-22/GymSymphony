using Gymphony.Application.Common.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class StaffController(IMediator mediator) : ControllerBase
{
    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> AddStaffAsync([FromBody] StaffSignUpCommand staffSignUpCommand, CancellationToken cancellationToken)
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