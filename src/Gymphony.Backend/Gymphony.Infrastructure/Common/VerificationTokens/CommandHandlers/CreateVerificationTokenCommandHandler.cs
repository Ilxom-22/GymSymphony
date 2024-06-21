using Gymphony.Application.Common.VerificationTokens.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Common.VerificationTokens.CommandHandlers;

public class CreateVerificationTokenCommandHandler(IServiceProvider serviceProvider) 
    : ICommandHandler<CreateVerificationTokenCommand, VerificationToken>
{
    public async Task<VerificationToken> Handle(CreateVerificationTokenCommand request, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var verificationTokenRepository = scope.ServiceProvider.GetRequiredService<IVerificationTokenRepository>();

        var oldToken = await verificationTokenRepository.GetByUserIdAsync(request.VerificationToken.UserId, cancellationToken: cancellationToken);

        if (oldToken is not null)
            await verificationTokenRepository.DeleteAsync(oldToken, cancellationToken: cancellationToken);
        
        return await verificationTokenRepository.CreateAsync(request.VerificationToken,
            cancellationToken: cancellationToken);
    }
}