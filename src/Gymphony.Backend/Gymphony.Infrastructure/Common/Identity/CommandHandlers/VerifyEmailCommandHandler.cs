using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class VerifyEmailCommandHandler(
    IUserRepository userRepository, 
    IVerificationTokenRepository verificationTokenRepository)
    : ICommandHandler<VerifyEmailCommand, bool>
{
    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var verificationToken = await verificationTokenRepository
            .GetByTokenAsync(request.Token, cancellationToken: cancellationToken)
            ?? throw new ArgumentException("Invalid or expired token!");

        if (verificationToken.ExpiryTime < DateTimeOffset.UtcNow)
            throw new ArgumentException("The verification token is expired!");

        var user = await userRepository.GetByIdAsync(verificationToken.UserId, cancellationToken: cancellationToken)
            ?? throw new EntityNotFoundException<User>($"User with id {verificationToken.UserId} not found!");

        user.Status = AccountStatus.Active;
        await userRepository.UpdateAsync(user, cancellationToken: cancellationToken);

        await verificationTokenRepository.DeleteAsync(verificationToken, cancellationToken: cancellationToken);

        return true;
    }
}