using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Queries;
using Gymphony.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IEventBusBroker eventBusBroker) : ControllerBase
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

    [HttpPost("refresh-token")]
    public async ValueTask<IActionResult> RefreshTokenAsync(
        [FromBody] RefreshTokenCommand refreshTokenCommand,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(refreshTokenCommand, cancellationToken);
        
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

    [Authorize]
    [HttpPost("change-password")]
    public async ValueTask<IActionResult> ChangePasswordAsync(
        [FromBody] ChangePasswordCommand changePasswordCommand,
        CancellationToken cancellationToken)
    {
        await mediator.Send(changePasswordCommand, cancellationToken);
        
        return NoContent();
    }

    [HttpPost("verify-email")]
    public async ValueTask<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailCommand verifyEmailCommand, CancellationToken cancellationToken)
    {
        await mediator.Send(verifyEmailCommand, cancellationToken);

        return Ok();
    }

    [HttpGet("resend-email-verification-message")]
    public async ValueTask<IActionResult> ResendVerificationEmail(
        [FromQuery] ResendEmailVerificationMessageCommand resendEmailVerificationMessageCommand,
        CancellationToken cancellationToken)
    {
        await mediator.Send(resendEmailVerificationMessageCommand, cancellationToken);
        
        return NoContent();
    }

    [HttpPost("forgot-password/{emailAddress}")]
    public async ValueTask<IActionResult> ForgotPassword(
        [FromRoute] string emailAddress,
        CancellationToken cancellationToken)
    {
        var forgotPasswordEvent = new ForgotPasswordEvent { EmailAddress = emailAddress };
        await eventBusBroker.PublishLocalAsync(forgotPasswordEvent, cancellationToken);
        
        return Accepted();
    }

    [HttpPost("reset-password")]
    public async ValueTask<IActionResult> ResetPassword(
        [FromBody] ResetPasswordCommand resetPasswordCommand,
        CancellationToken cancellationToken)
    {
        await mediator.Send(resetPasswordCommand, cancellationToken);
        
        return NoContent();
    }
}