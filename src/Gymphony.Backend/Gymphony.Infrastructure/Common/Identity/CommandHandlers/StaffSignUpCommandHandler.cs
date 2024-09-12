using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class StaffSignUpCommandHandler(IMapper mapper,
    IValidator<StaffSignUpCommand> staffSignUpCommandValidator,
    IPasswordHasherService passwordHasherService,
    IStaffRepository staffRepository,
    IMediator mediator)
    : ICommandHandler<StaffSignUpCommand, StaffDto>
{
    public async Task<StaffDto> Handle(StaffSignUpCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await staffSignUpCommandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        await staffRepository.BeginTransactionAsync();

        try
        {
            var staff = mapper.Map<Staff>(request);
            staff.AuthDataHash = passwordHasherService.HashPassword(Random.Shared.Next(10000000, 99999999).ToString());
            staff.Role = Role.Staff;
            staff.Status = AccountStatus.Active;

            await staffRepository.CreateAsync(staff, cancellationToken: cancellationToken);

            var uploadStaffProfileImageCommand = mapper.Map<UploadStaffProfileImageCommand>(request.ProfileImage);
            uploadStaffProfileImageCommand.StaffId = staff.Id;

            var profileImage = await mediator.Send(uploadStaffProfileImageCommand, cancellationToken);
            var staffDto = mapper.Map<StaffDto>(staff);
            staffDto.ProfileImage = profileImage;

            await staffRepository.CommitTransactionAsync();

            return staffDto;
        }
        catch
        {
            await staffRepository.RollbackTransactionAsync();
            throw;
        }
    }
}