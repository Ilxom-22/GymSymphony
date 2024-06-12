using System.Security.Authentication;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class LogOutCommandHandler(
    IRequestContextProvider requestContextProvider,
    IAccessTokenRepository accessTokenRepository,
    IRefreshTokenRepository refreshTokenRepository) 
    : ICommandHandler<LogOutCommand, bool>
{
    public async Task<bool> Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        var userId = requestContextProvider.GetUserId()
            ?? throw new AuthenticationException("Unauthorized access!");

        await accessTokenRepository.DeleteByUserIdAsync(userId, cancellationToken: cancellationToken);
        await refreshTokenRepository.DeleteByUserIdAsync(userId, cancellationToken: cancellationToken);

        return true;
    }
}