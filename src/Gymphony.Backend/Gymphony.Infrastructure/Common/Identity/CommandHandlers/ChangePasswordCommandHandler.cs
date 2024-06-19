using System.Security.Authentication;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class ChangePasswordCommandHandler(
    IRequestContextProvider requestContextProvider,
    IPasswordHasherService passwordHasherService,
    IUserRepository userRepository)
    : ICommandHandler<ChangePasswordCommand, bool>
{
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized access!");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken: cancellationToken)
            ?? throw new AuthenticationException("Unauthorized access!");

        if (!passwordHasherService.ValidatePassword(request.OldPassword, user.AuthDataHash))
            throw new AuthenticationException("Invalid old password!");

        if (passwordHasherService.ValidatePassword(request.NewPassword, user.AuthDataHash))
            throw new AuthenticationException("New Password Cannot Be the Same as Old Password!");

        user.AuthDataHash = passwordHasherService.HashPassword(request.NewPassword);
        await userRepository.UpdateAsync(user, cancellationToken: cancellationToken);
        
        return true;
    }
}