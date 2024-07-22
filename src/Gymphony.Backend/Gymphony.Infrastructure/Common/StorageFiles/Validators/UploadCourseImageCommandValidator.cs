using FluentValidation;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Application.Common.StorageFiles.Models.Settings;
using Gymphony.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Gymphony.Infrastructure.Common.StorageFiles.Validators;

public class UploadCourseImageCommandValidator : AbstractValidator<UploadCourseImageCommand>
{
    public UploadCourseImageCommandValidator(IOptions<StorageFileSettings> storageFileSettings)
    {
        RuleFor(command => command.CourseId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(command => command)
            .Custom((command, context) =>
        {
            var validationSetings = GetStorageFileSettingsByFileType(StorageFileType.CourseImage, storageFileSettings.Value);

            var type = command.ContentType.Split('/');

            if (!(type is ["image", _, ..] && validationSetings.AllowedImageExtensions.Contains(type[1])))
                context.AddFailure($"Unsupported file type: {command.ContentType}");

            if (command.Size > validationSetings.MaximumImageSizeInBytes)
                context.AddFailure($"File is too big!");

            if (command.Size < validationSetings.MinimumImageSizeInBytes)
                context.AddFailure($"File is too small!");
        });
    }

    private static AzureBlobStorageSettings GetStorageFileSettingsByFileType(StorageFileType type, StorageFileSettings storageFileSettings)
    {
        return storageFileSettings.Settings.Single(image =>
            image.StorageFileType == type);
    }
}
