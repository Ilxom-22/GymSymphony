using FluentAssertions;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Infrastructure.Common.Identity.CommandHandlers;
using Gymphony.Persistence.Repositories.Interfaces;
using NSubstitute;

namespace Gymphony.Tests.AuthUnitTests;

public class RemoveAdminCommandHandlerTests
{
    private readonly IEventBusBroker _eventBusBroker = Substitute.For<IEventBusBroker>();
    private readonly IRequestContextProvider _requestContextProvider = Substitute.For<IRequestContextProvider>();
    private readonly IAdminRepository _adminRepository = Substitute.For<IAdminRepository>();
    private readonly RemoveAdminCommandHandler _handler;

    public RemoveAdminCommandHandlerTests()
    {
        _handler = new RemoveAdminCommandHandler(_eventBusBroker, _requestContextProvider, _adminRepository);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidEntityStateChangeException_WhenAdminTriesToRemoveThemselvesAndIsOnlyActiveAdmin()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        var command = new RemoveAdminCommand { AdminId = adminId };

        _requestContextProvider.GetUserIdFromClaims().Returns(adminId);
        _adminRepository.GetActiveAdminsCount().Returns(1);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidEntityStateChangeException<Admin>>()
            .WithMessage("You cannot remove yourself from admins since you are currently the only active administrator!");
    }

    [Fact]
    public async Task Handle_ShouldThrowEntityNotFoundException_WhenAdminDoesNotExist()
    {
        // Arrange
        var command = new RemoveAdminCommand { AdminId = Guid.NewGuid() };

        _requestContextProvider.GetUserIdFromClaims().Returns(Guid.NewGuid());
        _adminRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>()).Returns((Admin)null);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Admin>>()
            .WithMessage($"Admin with id {command.AdminId} not found!");
    }

    [Fact]
    public async Task Handle_ShouldRemoveAdminAndPublishEvent_WhenAdminRemovalIsSuccessful()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        var command = new RemoveAdminCommand { AdminId = adminId };
        var foundAdmin = new Admin { Id = adminId };

        _requestContextProvider.GetUserIdFromClaims().Returns(Guid.NewGuid());
        _adminRepository.GetActiveAdminsCount().Returns(2);
        _adminRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>()).Returns(foundAdmin);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        await _adminRepository.Received(1).DeleteAsync(foundAdmin, Arg.Is(true), Arg.Any<CancellationToken>());
        await _eventBusBroker.Received(1)
            .PublishLocalAsync(Arg.Is<AdminRemovedEvent>(e => e.RemovedAdmin == foundAdmin && e.RemovedByAdminId != adminId),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task Handle_ShouldNotThrowException_WhenRemovingAnotherAdmin()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        var command = new RemoveAdminCommand { AdminId = adminId };
        var foundAdmin = new Admin { Id = adminId };

        _requestContextProvider.GetUserIdFromClaims().Returns(Guid.NewGuid());
        _adminRepository.GetActiveAdminsCount().Returns(2);
        _adminRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>()).Returns(foundAdmin);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenActiveAdminCountCheckFails()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        var command = new RemoveAdminCommand { AdminId = adminId };
        _requestContextProvider.GetUserIdFromClaims().Returns(adminId);
        _adminRepository.GetActiveAdminsCount().Returns(2);

        // Simulate failure in GetActiveAdminsCount
        _adminRepository.When(x => x.GetActiveAdminsCount()).Do(x => throw new Exception("DB Error"));

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("DB Error");
    }
}
