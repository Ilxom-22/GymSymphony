using Bogus;
using FluentAssertions;
using FluentValidation;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Infrastructure.Common.Identity.CommandHandlers;
using Gymphony.Infrastructure.Common.Identity.Validators;
using Gymphony.Persistence.Repositories.Interfaces;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Linq.Expressions;
using System.Security.Authentication;

namespace Gymphony.Tests.AuthUnitTests;

public class SignInCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IAccessTokenGeneratorService _accessTokenGeneratorService = Substitute.For<IAccessTokenGeneratorService>();
    private readonly IRefreshTokenGeneratorService _refreshTokenGeneratorService = Substitute.For<IRefreshTokenGeneratorService>();
    private readonly IPasswordHasherService _passwordHasherService = Substitute.For<IPasswordHasherService>();
    private readonly IValidator<SignInDetails> _signInDetailsValidator = new SignInDetailsValidator();
    private readonly Faker _faker = new Faker();
    private readonly SignInCommandHandler _handler;
   

    public SignInCommandHandlerTests()
    {
        _handler = new SignInCommandHandler(_userRepository, _accessTokenGeneratorService, _refreshTokenGeneratorService, _passwordHasherService, _signInDetailsValidator);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenSignInDetailsAreInvalid()
    {
        // Arrange
        var signInDetails = new SignInDetails { EmailAddress = "josh.com", AuthData = _faker.Internet.Password() };

        var command = new SignInCommand { SignInDetails = signInDetails };

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("Invalid Email Address! Please try again!");
    }

    [Fact]
    public async Task Handle_ShouldThrowAuthenticationException_WhenUserNotFound()
    {
        // Arrange
        var signInDetails = new SignInDetails { EmailAddress = _faker.Internet.Email(), AuthData = _faker.Internet.Password() };

        var command = new SignInCommand { SignInDetails = signInDetails };

        _userRepository.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(Enumerable.Empty<User>().AsQueryable().BuildMock());

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Provided login details are invalid! Please, try again!");
    }

    [Fact]
    public async Task Handle_ShouldThrowAuthenticationException_WhenPasswordIsInvalid()
    {
        // Arrange
        var signInDetails = new SignInDetails { EmailAddress = _faker.Internet.Email(), AuthData = _faker.Internet.Password() };

        var command = new SignInCommand { SignInDetails = signInDetails };

        var users = new List<User> { new Member { EmailAddress = signInDetails.EmailAddress, AuthDataHash = "invalid_hash" } };
        _userRepository.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(users.AsQueryable().BuildMock());

        _passwordHasherService.ValidatePassword(signInDetails.AuthData, "invalid_hash").Returns(false);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Provided login details are invalid! Please, try again!");
    }

    [Fact]
    public async Task Handle_ShouldThrowAuthenticationException_WhenUserIsBlocked()
    {
        // Arrange
        var signInDetails = new SignInDetails { EmailAddress = _faker.Internet.Email(), AuthData = _faker.Internet.Password() };

        var command = new SignInCommand { SignInDetails = signInDetails };

        var users = new List<User> { new Admin { EmailAddress = signInDetails.EmailAddress, AuthDataHash = "valid_hash", Status = AccountStatus.Blocked } };
        _userRepository.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(users.AsQueryable().BuildMock());

        _passwordHasherService.ValidatePassword(signInDetails.AuthData, "valid_hash").Returns(true);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Your account has been blocked. Please wait for an administrator to unblock it. We apologize for any inconvenience this may cause.");
    }

    [Fact]
    public async Task Handle_ShouldActivateUnverifiedAdmin_WhenUserIsUnverifiedAdmin()
    {
        // Arrange
        var signInDetails = new SignInDetails { EmailAddress = _faker.Internet.Email(), AuthData = _faker.Internet.Password() };

        var command = new SignInCommand { SignInDetails = signInDetails };

        var admin = new Admin { EmailAddress = signInDetails.EmailAddress, AuthDataHash = "valid_hash", Status = AccountStatus.Unverified, Role = Role.Admin };
        var users = new List<Admin> { admin };
        _userRepository.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(users.AsQueryable().BuildMock());

        _passwordHasherService.ValidatePassword(signInDetails.AuthData, "valid_hash").Returns(true);

        var accessToken = new AccessToken { Token = _faker.Random.String(), ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(30) };
        var refreshToken = new RefreshToken { Token = _faker.Random.String(), ExpiryTime = DateTimeOffset.UtcNow.AddDays(7) };

        _accessTokenGeneratorService.GetAccessToken(admin).Returns(accessToken);
        _refreshTokenGeneratorService.GenerateRefreshToken(admin).Returns(refreshToken);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        admin.Status.Should().Be(AccountStatus.Active);
        await _userRepository.Received(1).UpdateAsync(Arg.Is<Admin>(a => a == admin && a.Status == AccountStatus.Active));
    }

    [Fact]
    public async Task Handle_ShouldReturnIdentityToken_WhenSignInIsSuccessful()
    {
        // Arrange
        var signInDetails = new SignInDetails { EmailAddress = _faker.Internet.Email(), AuthData = _faker.Internet.Password() };

        var command = new SignInCommand { SignInDetails = signInDetails };

        User user = new Member { EmailAddress = signInDetails.EmailAddress, AuthDataHash = "valid_hash", Status = AccountStatus.Active };
        var users = new List<User> { user };
        _userRepository.Get(Arg.Any<Expression<Func<User, bool>>>()).Returns(users.AsQueryable().BuildMock());

        _passwordHasherService.ValidatePassword(signInDetails.AuthData, "valid_hash").Returns(true);

        var accessToken = new AccessToken { Token = _faker.Random.String(), ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(30) };
        var refreshToken = new RefreshToken { Token = _faker.Random.String(), ExpiryTime = DateTimeOffset.UtcNow.AddDays(7) };

        _accessTokenGeneratorService.GetAccessToken(user).Returns(accessToken);
        _refreshTokenGeneratorService.GenerateRefreshToken(user).Returns(refreshToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be(accessToken.Token);
        result.RefreshToken.Should().Be(refreshToken.Token);

        await _userRepository.Received(1).UpdateAsync(user, Arg.Is(true), Arg.Any<CancellationToken>());
    }   
}
