using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasherService passwordHasherService)
    : ICommandHandler<ResetPasswordCommand, bool>
{
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.Get(user => user.VerificationToken.Token == request.Token)
            .Include(user => user.VerificationToken)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException("Password reset token is either invalid or expired!");

        if (user.VerificationToken?.ExpiryTime < DateTimeOffset.UtcNow)
            throw new ArgumentException("Password reset token is expired!");

        if (passwordHasherService.ValidatePassword(request.NewPassword, user.AuthDataHash))
            throw new ArgumentException("New password can't be the same as an old password!");

        user.AuthDataHash = passwordHasherService.HashPassword(request.NewPassword);
        user.AuthenticationProvider = Provider.EmailPassword;
        user.VerificationToken = null;

        await userRepository.UpdateAsync(user, cancellationToken: cancellationToken);

        return true;
    }
}