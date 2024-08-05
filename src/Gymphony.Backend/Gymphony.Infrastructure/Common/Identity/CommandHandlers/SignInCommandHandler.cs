using System.Security.Authentication;
using FluentValidation;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class SignInCommandHandler(
    IUserRepository userRepository,
    IAccessTokenGeneratorService accessTokenGeneratorService,
    IRefreshTokenGeneratorService refreshTokenGeneratorService,
    IPasswordHasherService passwordHasherService,
    IValidator<SignInDetails> signInDetailsValidator)
    : ICommandHandler<SignInCommand, IdentityTokenDto>
{
    public async Task<IdentityTokenDto> Handle(
        SignInCommand request, 
        CancellationToken cancellationToken)
    {
        var validatedUser = await ValidateSignInDetails(request.SignInDetails, cancellationToken);

        return await GetIdentityToken(validatedUser, cancellationToken);
    }

    private async ValueTask<User> ValidateSignInDetails(SignInDetails signInDetails, CancellationToken cancellationToken = default)
    {
        var validationResult = await signInDetailsValidator
            .ValidateAsync(signInDetails, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors[0].ToString());
        
        var foundUser = await userRepository.Get(user => user.EmailAddress == signInDetails.EmailAddress)
                            .Include(user => user.AccessToken)
                            .Include(user => user.RefreshToken)
                            .FirstOrDefaultAsync(cancellationToken)
                        ?? throw new AuthenticationException("Provided login details are invalid! Please, try again!");

        if (!passwordHasherService.ValidatePassword(signInDetails.AuthData, foundUser.AuthDataHash))
            throw new ValidationException("Provided login details are invalid! Please, try again!");

        if (foundUser.Status == AccountStatus.Blocked)
            throw new AuthenticationException(
                "Your account has been blocked. Please wait for an administrator to unblock it. We apologize for any inconvenience this may cause.");
        
        if (foundUser is { Role: Role.Admin, Status: AccountStatus.Unverified })
            foundUser.Status = AccountStatus.Active;
        
        return foundUser;
    }

    private async ValueTask<IdentityTokenDto> GetIdentityToken(
        User user,
        CancellationToken cancellationToken)
    {
        user.AccessToken = user.AccessToken?.ExpiryTime > DateTimeOffset.UtcNow
            ? user.AccessToken
            : accessTokenGeneratorService.GetAccessToken(user);

        user.RefreshToken = user.RefreshToken?.ExpiryTime > DateTimeOffset.UtcNow
            ? user.RefreshToken
            : refreshTokenGeneratorService.GenerateRefreshToken(user);

        await userRepository.UpdateAsync(user, cancellationToken: cancellationToken);

        return new IdentityTokenDto
        {
            AccessToken = user.AccessToken.Token,
            RefreshToken = user.RefreshToken.Token
        };
    }
}