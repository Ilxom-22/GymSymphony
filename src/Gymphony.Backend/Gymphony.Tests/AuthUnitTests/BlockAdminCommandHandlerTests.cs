using FluentAssertions;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Infrastructure.Common.Identity.CommandHandlers;
using Gymphony.Persistence.Repositories.Interfaces;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Linq.Expressions;
using System.Security.Authentication;

namespace Gymphony.Tests.AuthUnitTests;

public class BlockAdminCommandHandlerTests
{
    private readonly IRequestContextProvider _requestContextProvider = Substitute.For<IRequestContextProvider>();
    private readonly IEventBusBroker _eventBusBroker = Substitute.For<IEventBusBroker>();
    private readonly IAdminRepository _adminRepository = Substitute.For<IAdminRepository>();
    private readonly Guid _adminId = Guid.NewGuid();
    private readonly BlockAdminCommandHandler _handler;

    public BlockAdminCommandHandlerTests()
    {
        _handler = new BlockAdminCommandHandler(_requestContextProvider, _eventBusBroker, _adminRepository);
    }

    [Fact]
    public async Task Handle_ShouldThrowAuthenticationException_WhenUserIsUnauthorized()
    {
        // Arrange
        var command = new BlockAdminCommand { AdminId = _adminId };
        _requestContextProvider.GetUserIdFromClaims().Returns((Guid?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Unauthorized access!");
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidEntityStateChangeException_WhenSingleAdminTriesToBlockHimself()
    {
        // Arrange
        var command = new BlockAdminCommand { AdminId = _adminId };
        _requestContextProvider.GetUserIdFromClaims().Returns(_adminId);
        _adminRepository.GetActiveAdminsCount().Returns(1);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidEntityStateChangeException<Admin>>()
            .WithMessage("You cannot block yourself since you are currently the only active administrator!");
    }

    [Fact]
    public async Task Handle_ShouldThrowEntityNotFoundException_WhenGivenAdminDoesNotExist()
    {
        var command = new BlockAdminCommand { AdminId = _adminId };
        _requestContextProvider.GetUserIdFromClaims().Returns(_adminId);
        _adminRepository.GetActiveAdminsCount().Returns(2);

        var emptyAdmins = Enumerable.Empty<Admin>().AsQueryable();
        var mock = emptyAdmins.BuildMock();

        _adminRepository.Get(Arg.Any<Expression<Func<Admin, bool>>>()).Returns(mock);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Admin>>()
            .WithMessage($"Admin with id {_adminId} not found!");
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenAdminIsAlreadyBlocked()
    {
        // Arrange
        var command = new BlockAdminCommand { AdminId = _adminId };
        _requestContextProvider.GetUserIdFromClaims().Returns(_adminId);
        _adminRepository.GetActiveAdminsCount().Returns(2);

        var admins = new List<Admin> { new Admin { Id = _adminId, Status = AccountStatus.Blocked } };
        var mock = admins.AsQueryable().BuildMock();

        _adminRepository.Get(Arg.Any<Expression<Func<Admin, bool>>>()).Returns(mock);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        await _adminRepository.DidNotReceive().UpdateAsync(Arg.Any<Admin>(), Arg.Is(true), Arg.Any<CancellationToken>());
        await _eventBusBroker.DidNotReceive().PublishLocalAsync(Arg.Any<AdminBlockedEvent>(), Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(AccountStatus.Active, AccountStatus.Blocked, 2)]
    [InlineData(AccountStatus.Active, AccountStatus.Blocked, 3)]
    public async Task Handle_ShouldReturnTrue_WhenRequestIsValid(AccountStatus initialStatus, AccountStatus finalStatus, int activeAdminsCount)
    {
        // Arrange
        var command = new BlockAdminCommand { AdminId = _adminId };
        _requestContextProvider.GetUserIdFromClaims().Returns(_adminId);
        _adminRepository.GetActiveAdminsCount().Returns(activeAdminsCount);

        var admins = new List<Admin> { new() { Id = _adminId, Status = initialStatus } };
        var mock = admins.AsQueryable().BuildMock();

        _adminRepository.Get(Arg.Any<Expression<Func<Admin, bool>>>()).Returns(mock);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        await _adminRepository.Received(1)
            .UpdateAsync(Arg.Is<Admin>(admin => admin == admins[0] && admin.Status == AccountStatus.Blocked),
            Arg.Is(true), Arg.Any<CancellationToken>());

        await _eventBusBroker.Received(1).PublishLocalAsync(Arg.Is<AdminBlockedEvent>(e =>
            e.BlockedAdmin.Id == _adminId &&
            e.BlockedAdmin.Status == finalStatus), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenMultipleActiveAdmins()
    {
        // Arrange
        var command = new BlockAdminCommand { AdminId = _adminId };
        _requestContextProvider.GetUserIdFromClaims().Returns(Guid.NewGuid());
        _adminRepository.GetActiveAdminsCount().Returns(3);

        var admins = new List<Admin> { new Admin { Id = _adminId, Status = AccountStatus.Active } };
        var mock = admins.AsQueryable().BuildMock();

        _adminRepository.Get(Arg.Any<Expression<Func<Admin, bool>>>()).Returns(mock);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        await _adminRepository.Received(1)
           .UpdateAsync(Arg.Is<Admin>(admin => admin == admins[0] && admin.Status == AccountStatus.Blocked),
           Arg.Is(true), Arg.Any<CancellationToken>());

        await _eventBusBroker.Received(1).PublishLocalAsync(Arg.Is<AdminBlockedEvent>(e =>
            e.BlockedAdmin.Id == _adminId &&
            e.BlockedAdmin.Status == AccountStatus.Blocked), Arg.Any<CancellationToken>());
    }
}
