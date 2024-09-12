using System.Security.Authentication;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class RefreshTokenCommandHandler(
    IRequestContextProvider requestContextProvider,
    IAccessTokenGeneratorService accessTokenGeneratorService,
    IAccessTokenRepository accessTokenRepository,
    IRefreshTokenGeneratorService refreshTokenGeneratorService,
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository) 
    : ICommandHandler<RefreshTokenCommand, IdentityTokenDto>
{
    public async Task<IdentityTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new ArgumentException("Invalid refresh token!");

        var refreshToken = await refreshTokenRepository.GetByValueAsync(request.RefreshToken, 
                               new QueryOptions(QueryTrackingMode.AsNoTracking))
                           ?? throw new AuthenticationException("Provided refresh token is invalid. Try to login again, please.");

        if (refreshToken.ExpiryTime < DateTimeOffset.UtcNow)
            throw new AuthenticationException("Refresh token is expired! Please login again!");
        
        var accessToken = requestContextProvider.GetAccessToken();
        var actionUserId = requestContextProvider.GetUserIdFromClaimsOrToken();

        if (string.IsNullOrWhiteSpace(accessToken) || actionUserId is null)
            await DeleteTokensAndThrowException(refreshToken, false);

        if (actionUserId != refreshToken.UserId)
            await DeleteTokensAndThrowException(refreshToken);

        var actualAccessToken = await accessTokenRepository
            .GetByUserIdAsync((Guid)actionUserId!, cancellationToken: cancellationToken);

        if (actualAccessToken is null)
            await DeleteTokensAndThrowException(refreshToken, false);

        if (actualAccessToken!.Token != accessToken)
            await DeleteTokensAndThrowException(refreshToken);

        var user = await userRepository.GetByIdAsync((Guid)actionUserId, cancellationToken: cancellationToken);

        if (actualAccessToken.ExpiryTime > DateTimeOffset.UtcNow)
            return new IdentityTokenDto
            {
                AccessToken = actualAccessToken.Token,
                RefreshToken = (await GetNewRefreshToken(user!, cancellationToken)).Token
            };

        return await GenerateTokens(user!, cancellationToken);
    }
    
    private async Task DeleteTokensAndThrowException(RefreshToken refreshToken, bool removeAccessToken = true)
    {
        await refreshTokenRepository.DeleteAsync(refreshToken);
        
        if (removeAccessToken)
            await accessTokenRepository.DeleteByUserIdAsync(refreshToken.UserId);
        
        throw new AuthenticationException("Invalid identity security token!");
    }

    private async ValueTask<RefreshToken> GetNewRefreshToken(User user, CancellationToken cancellationToken = default)
    {
        var refreshToken = refreshTokenGeneratorService.GenerateRefreshToken(user);

        await refreshTokenRepository.DeleteByUserIdAsync(user.Id, cancellationToken: cancellationToken);
        return await refreshTokenRepository.CreateAsync(refreshToken, cancellationToken: cancellationToken);
    }

    private async ValueTask<IdentityTokenDto> GenerateTokens(User user, CancellationToken cancellationToken = default)
    {
        var refreshToken = await GetNewRefreshToken(user, cancellationToken);
        var accessToken = accessTokenGeneratorService.GetAccessToken(user);

        await accessTokenRepository.DeleteByUserIdAsync(user.Id, cancellationToken: cancellationToken);
        await accessTokenRepository.CreateAsync(accessToken, cancellationToken: cancellationToken);

        return new IdentityTokenDto
        {
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.Token
        };
    }
}