using AutoMapper;
using FluentAssertions;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Queries;
using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Infrastructure.Common.Identity.QueryHandlers;
using Gymphony.Persistence.Repositories.Interfaces;
using MockQueryable.NSubstitute;
using NSubstitute;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Authentication;

namespace Gymphony.Tests.AuthUnitTests;

public class GetCurrentUserQueryHandlerTests
{
    private readonly IRequestContextProvider _requestContextProvider = Substitute.For<IRequestContextProvider>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IAdminRepository _adminRepository = Substitute.For<IAdminRepository>();
    private readonly GetCurrentUserQueryHandler _handler;

    public GetCurrentUserQueryHandlerTests()
    {
        _handler = new GetCurrentUserQueryHandler(_requestContextProvider, _mapper, _userRepository, _adminRepository);
    }

    [Fact]
    public async Task Handle_ShouldThrowAuthenticationException_WhenUserIsUnauthorized()
    {
        // Arrange
        var query = new GetCurrentUserQuery();

        _requestContextProvider.GetUserIdFromClaims().Returns((Guid?)null);

        // Act
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Unauthorized access!");
    }

    [Fact]
    public async Task Handle_ShouldThrowAuthenticationException_WhenUserIsNotFoundInDatabase()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetCurrentUserQuery();

        _requestContextProvider.GetUserIdFromClaims().Returns(userId);
        _userRepository.Get(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<QueryOptions>())
            .Returns(Enumerable.Empty<User>().AsQueryable().BuildMock());

        // Act
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Unauthorized access!");
    }

    [Fact]
    public async Task Handle_ShouldReturnUserDto_WhenUserIsFoundInDatabase()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetCurrentUserQuery();

        var user = new Admin { Id = userId, EmailAddress = "test@example.com" };
        var users = new List<User> { user }.AsQueryable().BuildMock();
        var admins = new List<Admin> { user }.AsQueryable().BuildMock();

        _requestContextProvider.GetUserIdFromClaims().Returns(userId);
        _userRepository.Get(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<QueryOptions>())
            .Returns(users);
        _adminRepository.Get(Arg.Any<Expression<Func<Admin, bool>>>(), Arg.Any<QueryOptions>())
            .Returns(admins);

        var userDto = new UserDto { Id = userId, EmailAddress = "test@example.com" };
        _mapper.Map<UserDto>(user).Returns(userDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnUserDtoWithProfileImage_WhenUserExistsInDatabase()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetCurrentUserQuery();

        var profileImage = new UserProfileImage { StorageFile = new StorageFile { Id = Guid.NewGuid(), Url = "url/to/image" } };
        var user = new Admin { Id = userId, EmailAddress = "test@example.com", ProfileImage = profileImage };
        var admins = new List<Admin> { user }.AsQueryable().BuildMock();
        var users = new List<User> { user }.AsQueryable().BuildMock();
        _adminRepository.Get(Arg.Any<Expression<Func<Admin, bool>>>(), Arg.Any<QueryOptions>())
           .Returns(admins);

        _requestContextProvider.GetUserIdFromClaims().Returns(userId);
        _userRepository.Get(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<QueryOptions>())
            .Returns(users);

        var profileImageDto = new UserProfileImageDto { ProfileImageId = Guid.NewGuid(), Url = "url/to/image" };
        var userDto = new UserDto { Id = userId, EmailAddress = "test@example.com", ProfileImage = profileImageDto };
        _mapper.Map<UserDto>(user).Returns(userDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(userDto);
        result.ProfileImage.Should().BeEquivalentTo(profileImageDto);
    }
}
