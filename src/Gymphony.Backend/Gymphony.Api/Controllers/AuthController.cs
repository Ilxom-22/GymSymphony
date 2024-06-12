using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Queries;
using Gymphony.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async ValueTask<IActionResult> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var getCurrentUserQuery = new GetCurrentUserQuery();

        var result = await mediator.Send(getCurrentUserQuery, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("sign-up-by-email")]
    public async ValueTask<IActionResult> SignUpWithEmailAsync(
        [FromBody] SignUpDetails signUpDetails,
        CancellationToken cancellationToken = default)
    {
        var signUpCommand = new SignUpCommand
        { 
            SignUpDetails = signUpDetails,
            AuthProvider = Provider.EmailPassword,
            Role = Role.Member
        };

        var result = await mediator.Send(signUpCommand, cancellationToken);
        
        return Ok(result);
    }

    [HttpPost("sign-in-by-email")]
    public async ValueTask<IActionResult> SignInWithEmailAsync(
        [FromBody] SignInDetails signInDetails, 
        CancellationToken cancellationToken = default)
    {
        var signInCommand = new SignInCommand
        {
            SignInDetails = signInDetails,
            AuthProvider = Provider.EmailPassword
        };

        var result = await mediator.Send(signInCommand, cancellationToken);
        
        return Ok(result);
    }

    [Authorize]
    [HttpPost("log-out")]
    public async ValueTask<IActionResult> LogOutAsync(CancellationToken cancellationToken)
    {
        var logoutCommand = new LogOutCommand();
        await mediator.Send(logoutCommand, cancellationToken);
        
        return NoContent();
    }
}