using Bogus;
using FluentAssertions;
using FluentValidation;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Enums;
using Gymphony.Infrastructure.Common.Identity.CommandHandlers;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using NSubstitute;

namespace Gymphony.Tests.AuthUnitTests;

public class SignUpCommandHandlerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IValidator<SignUpDetails> _validator = Substitute.For<IValidator<SignUpDetails>>();
    private readonly SignUpCommandHandler _handler;
    private readonly Faker<SignUpDetails> _fakeSignUpDetails = new Faker<SignUpDetails>()
            .RuleFor(d => d.FirstName, f => f.Name.FirstName())
            .RuleFor(d => d.LastName, f => f.Name.LastName())
            .RuleFor(d => d.EmailAddress, f => f.Internet.Email())
            .RuleFor(d => d.AuthData, f => f.Internet.Password())
            .RuleFor(s => s.BirthDay, f => f.Date.PastOffset(30, DateTimeOffset.Now.AddYears(-18)));

    public SignUpCommandHandlerTests()
    {
        _handler = new SignUpCommandHandler(_mediator, _userRepository, _validator);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenUserAlreadyExists()
    {
        // Arrange
        var signUpDetails = _fakeSignUpDetails.Generate();
        var command = new SignUpCommand
        {
            SignUpDetails = signUpDetails,
            Role = Role.Member,
            AuthProvider = Provider.EmailPassword
        };

        _userRepository.UserExists(command.SignUpDetails.EmailAddress).Returns(true);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("User with this email address is already registered!");
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var signUpDetails = _fakeSignUpDetails.Generate();
        signUpDetails.EmailAddress = "josh.com";
        var command = new SignUpCommand
        {
            SignUpDetails = signUpDetails,
            Role = Role.Member,
            AuthProvider = Provider.EmailPassword
        };

        _userRepository.UserExists(command.SignUpDetails.EmailAddress).Returns(false);
        var validationFailure = new FluentValidation.Results.ValidationFailure("EmailAddress", "Invalid email");
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { validationFailure });
        _validator.ValidateAsync(signUpDetails, Arg.Any<CancellationToken>()).Returns(validationResult);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>()
            .WithMessage("Invalid email");
    }

    [Fact]
    public async Task Handle_ShouldSendAdminSignUpCommand_WhenRoleIsAdmin()
    {
        // Arrange
        var signUpDetails = _fakeSignUpDetails.Generate();
        var command = new SignUpCommand
        {
            SignUpDetails = signUpDetails,
            Role = Role.Admin,
            AuthProvider = Provider.EmailPassword
        };

        _userRepository.UserExists(command.SignUpDetails.EmailAddress).Returns(false);
        var validationResult = new FluentValidation.Results.ValidationResult();
        _validator.ValidateAsync(signUpDetails, Arg.Any<CancellationToken>()).Returns(validationResult);
        var expectedUserDto = new UserDto { EmailAddress = signUpDetails.EmailAddress };
        _mediator.Send(Arg.Any<AdminSignUpCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedUserDto));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedUserDto);
        await _mediator.Received(1).Send(Arg.Any<AdminSignUpCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldSendMemberSignUpCommand_WhenRoleIsMember()
    {
        // Arrange
        var signUpDetails = _fakeSignUpDetails.Generate();
        var command = new SignUpCommand
        {
            SignUpDetails = signUpDetails,
            Role = Role.Member,
            AuthProvider = Provider.EmailPassword
        };

        _userRepository.UserExists(command.SignUpDetails.EmailAddress).Returns(false);
        var validationResult = new FluentValidation.Results.ValidationResult();
        _validator.ValidateAsync(signUpDetails, Arg.Any<CancellationToken>()).Returns(validationResult);
        var expectedUserDto = new UserDto { EmailAddress = signUpDetails.EmailAddress };
        _mediator.Send(Arg.Any<MemberSignUpCommand>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedUserDto));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedUserDto);
        await _mediator.Received(1).Send(Arg.Any<MemberSignUpCommand>(), Arg.Any<CancellationToken>());
    }
}
