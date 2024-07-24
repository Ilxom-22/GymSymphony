using AutoMapper;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.StorageFiles.CommandHandlers;

public class UploadStaffProfileImageCommandHandler(IMapper mapper, IMediator mediator, IStaffRepository staffRepository)
    : ICommandHandler<UploadStaffProfileImageCommand, UserProfileImageDto>
{
    public async Task<UserProfileImageDto> Handle(UploadStaffProfileImageCommand request, CancellationToken cancellationToken)
    {
        var foundStaff = await staffRepository.Get(staff => staff.Id == request.StaffId)
            .Include(staff => staff.ProfileImage)
            .ThenInclude(pi => pi.StorageFile)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Staff with id {request.StaffId} does not exist!");

        var uploadProfileImageCommand = mapper.Map<UploadProfileImageCommand>(request);
        uploadProfileImageCommand.User = foundStaff;

        return await mediator.Send(uploadProfileImageCommand, cancellationToken);
    }
}
