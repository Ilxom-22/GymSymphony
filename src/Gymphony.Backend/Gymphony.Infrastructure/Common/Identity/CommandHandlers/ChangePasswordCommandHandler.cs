using System.Security.Authentication;
using FluentValidation;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class ChangePasswordCommandHandler(
    IRequestContextProvider requestContextProvider,
    IPasswordHasherService passwordHasherService,
    IUserRepository userRepository,
    IAdminRepository adminRepository,
    IValidator<string> passwordValidator)
    : ICommandHandler<ChangePasswordCommand, bool>
{
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized access!");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken: cancellationToken)
            ?? throw new AuthenticationException("Unauthorized access!");

        if (user.AuthenticationProvider != Provider.EmailPassword)
            throw new InvalidEntityStateChangeException<User>($"Since you signed up using your {user.AuthenticationProvider.ToString()} account, all the passwords are managed by your provider. You can change your password by visiting your account settings on {user.AuthenticationProvider.ToString()}");

        if (!passwordHasherService.ValidatePassword(request.OldPassword, user.AuthDataHash))
            throw new ArgumentException("Invalid old password!");

        if (passwordHasherService.ValidatePassword(request.NewPassword, user.AuthDataHash))
            throw new ArgumentException("New Password Cannot Be the Same as Old Password!");

        var validationResult = await passwordValidator.ValidateAsync(request.NewPassword, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        user.AuthDataHash = passwordHasherService.HashPassword(request.NewPassword);

        if (user.Role != Role.Admin)
        {
            await userRepository.UpdateAsync(user, cancellationToken: cancellationToken);

            return true;
        }

        var admin = (Admin)user;
        admin.TemporaryPasswordChanged = true;
        await adminRepository.UpdateAsync(admin, cancellationToken: cancellationToken);

        return true;
    }
}