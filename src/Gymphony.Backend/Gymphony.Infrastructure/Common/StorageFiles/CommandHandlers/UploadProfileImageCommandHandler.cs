using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.StorageFiles.Brokers;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.StorageFiles.CommandHandlers;

public class UploadProfileImageCommandHandler(
    IMapper mapper,
    IStorageFileRepository storageFileRepository,
    IUserRepository userRepository,
    IValidator<UploadProfileImageCommand> commandValidator,
    IAzureBlobStorageBroker azureBlobStorageBroker)
    : ICommandHandler<UploadProfileImageCommand, UserProfileImageDto>
{
    public async Task<UserProfileImageDto> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid) 
            throw new ArgumentException(validationResult.Errors[0].ToString());

        var lastProfileImage = request.User.ProfileImage;

        var uploadFileInfo = mapper.Map<UploadFileInfoDto>(request);
        var storageFile = await azureBlobStorageBroker.UploadAsync(uploadFileInfo, cancellationToken);

        var profileImage = new UserProfileImage
        {
            User = request.User,
            StorageFile = storageFile,
        };

        request.User.ProfileImage = profileImage;

        await userRepository.UpdateAsync(request.User, cancellationToken: cancellationToken);

        if (lastProfileImage is not null)
        {
            await azureBlobStorageBroker.DeleteAsync(lastProfileImage.StorageFile!, cancellationToken: cancellationToken);
            await storageFileRepository.DeleteAsync(lastProfileImage.StorageFile!, cancellationToken: cancellationToken);
        }

        return new UserProfileImageDto { ProfileImageId = profileImage.Id, Url = profileImage.StorageFile.Url };
    }
}
