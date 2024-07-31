using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.StorageFiles.Brokers;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.StorageFiles.CommandHandlers;

public class UploadCourseImageCommandHandler(IMapper mapper,
    ICourseRepository courseRepository,
    ICourseImageRepository courseImageRepository,
    IAzureBlobStorageBroker azureBlobStorageBroker,
    IValidator<UploadCourseImageCommand> commandValidator) 
    : ICommandHandler<UploadCourseImageCommand, CourseImageDto>
{
    public async Task<CourseImageDto> Handle(UploadCourseImageCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken: cancellationToken)
            ?? throw new ArgumentException($"Course with id {request.CourseId} does not exist!");

        var uploadFileInfo = mapper.Map<UploadFileInfoDto>(request);
        var storageFile = await azureBlobStorageBroker.UploadAsync(uploadFileInfo, cancellationToken);

        var courseImage = new CourseImage
        {
            Course = course,
            StorageFile = storageFile,
        };

        await courseImageRepository.CreateAsync(courseImage, cancellationToken: cancellationToken);

        return new CourseImageDto { CourseImageId = courseImage.Id, CourseImageUrl = courseImage.StorageFile.Url };
    }
}
