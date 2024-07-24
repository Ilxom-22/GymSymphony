using AutoMapper;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Gymphony.Infrastructure.Common.StorageFiles.CommandHandlers;

public class UploadUserProfileImageCommandHandler(
    IMediator mediator, IMapper mapper,
    IUserRepository userRepository,
    IRequestContextProvider requestContextProvider)
    : ICommandHandler<UploadUserProfileImageCommand, UserProfileImageDto>
{
    public async Task<UserProfileImageDto> Handle(UploadUserProfileImageCommand request, CancellationToken cancellationToken)
    {
        var userId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized access!");

        var foundUser = await userRepository.Get(user => user.Id == userId)
            .Include(user => user.ProfileImage)
            .ThenInclude(pi => pi.StorageFile)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AuthenticationException("Unauthorized access!");

        var uploadProfileImageCommand = mapper.Map<UploadProfileImageCommand>(request);
        uploadProfileImageCommand.User = foundUser;

        return await mediator.Send(uploadProfileImageCommand, cancellationToken);
    }
}
