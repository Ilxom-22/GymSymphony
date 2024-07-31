using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class StaffSignUpCommandHandler(IMapper mapper,
    IValidator<StaffSignUpCommand> staffSignUpCommandValidator,
    IPasswordHasherService passwordHasherService,
    IStaffRepository staffRepository)
    : ICommandHandler<StaffSignUpCommand, StaffDto>
{
    public async Task<StaffDto> Handle(StaffSignUpCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await staffSignUpCommandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        var staff = mapper.Map<Staff>(request);
        staff.AuthDataHash = passwordHasherService.HashPassword(Random.Shared.Next(10000000, 99999999).ToString());
        staff.Role = Role.Staff;

        await staffRepository.CreateAsync(staff, cancellationToken: cancellationToken);

        return mapper.Map<StaffDto>(staff);
    }
}