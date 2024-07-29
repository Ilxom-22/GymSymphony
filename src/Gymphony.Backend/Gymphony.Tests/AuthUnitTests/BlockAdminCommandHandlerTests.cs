using FluentAssertions;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Entities;
using Gymphony.Infrastructure.Common.Identity.CommandHandlers;
using Gymphony.Persistence.Repositories.Interfaces;
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
    public async Task Handle_ShouldThrowValidationException_WhenUserAlreadyExists()
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
        _adminRepository.Get(Arg.Any<Expression<Func<Admin, bool>>>())
                .Returns(Enumerable.Empty<Admin>().AsQueryable());

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Admin>>()
            .WithMessage($"Admin with id {_adminId} not found!");
    }
}
