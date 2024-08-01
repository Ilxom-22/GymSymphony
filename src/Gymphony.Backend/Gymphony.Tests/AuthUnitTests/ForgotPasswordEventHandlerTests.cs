using FluentAssertions;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Infrastructure.Common.Identity.EventHandlers;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Gymphony.Tests.AuthUnitTests;

public class ForgotPasswordEventHandlerTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ForgotPasswordEventHandler _handler;

    public ForgotPasswordEventHandlerTests()
    {
        var scope = Substitute.For<IServiceScope>();
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        var serviceProviderMock = Substitute.For<IServiceProvider>();

        scope.ServiceProvider.Returns(serviceProviderMock);
        scopeFactory.CreateScope().Returns(scope);

        serviceProviderMock.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);
        serviceProviderMock.GetService(typeof(IUserRepository)).Returns(_userRepository);

        _serviceProvider = serviceProviderMock;

        _handler = new ForgotPasswordEventHandler(_serviceProvider, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenUserNotFoundInDatabase()
    {
        // Arrange
        var request = new ForgotPasswordEvent { EmailAddress = "nonexistent@example.com" };

        _userRepository.GetByEmailAddressAsync(request.EmailAddress, Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<User?>(null));

        // Act
        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"User with email address {request.EmailAddress} does not exist!");
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidEntityStateChangeException_WhenUserNotUsingEmailPasswordProvider()
    {
        // Arrange
        var request = new ForgotPasswordEvent { EmailAddress = "test@example.com" };

        var user = new Member { EmailAddress = request.EmailAddress, AuthenticationProvider = Provider.Google };

        _userRepository.GetByEmailAddressAsync(request.EmailAddress, Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<User?>(user));

        // Act
        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidEntityStateChangeException<User>>()
            .WithMessage($"Since you signed up using your {user.AuthenticationProvider} account, all the passwords are managed by your provider. Please try to sign in using your {user.AuthenticationProvider} account!");
    }

    [Fact]
    public async Task Handle_ShouldPublishPasswordResetNotification_WhenUserExistsAndUsesEmailPasswordProvider()
    {
        // Arrange
        var request = new ForgotPasswordEvent { EmailAddress = "test@example.com" };

        var user = new Admin { EmailAddress = request.EmailAddress, AuthenticationProvider = Provider.EmailPassword };

        _userRepository.GetByEmailAddressAsync(request.EmailAddress, Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<User?>(user));

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _mediator.Received(1)
            .Publish(Arg.Is<PasswordResetNotificationRequestedEvent>(e => e.Recipient == user), Arg.Any<CancellationToken>());
    }
}
