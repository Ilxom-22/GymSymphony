using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminsController(IMediator mediator) : ControllerBase
{
    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> AddAdminAsync(
        [FromBody] SignUpDetails signUpDetails,
        CancellationToken cancellationToken)
    {
        var signUpCommand = new SignUpCommand
        {
            SignUpDetails = signUpDetails,
            AuthProvider = Provider.EmailPassword,
            Role = Role.Admin
        };

        var result = await mediator.Send(signUpCommand, cancellationToken);

        return Ok(result);
    }

    [HttpPut("block/{adminId:guid}")]
    public async ValueTask<IActionResult> BlockAdminAsync(
        [FromRoute] Guid adminId,
        CancellationToken cancellationToken)
    {
        var blockAdminCommand = new BlockAdminCommand { AdminId = adminId };

        await mediator.Send(blockAdminCommand, cancellationToken);
        
        return NoContent();
    }

    [HttpPut("unblock/{adminId:guid}")]
    public async ValueTask<IActionResult> UnblockAdminAsync(
        [FromRoute] Guid adminId,
        CancellationToken cancellationToken)
    {
        var unblockAdminCommand = new UnblockAdminCommand { AdminId = adminId };

        await mediator.Send(unblockAdminCommand, cancellationToken);
        
        return NoContent();
    }
}