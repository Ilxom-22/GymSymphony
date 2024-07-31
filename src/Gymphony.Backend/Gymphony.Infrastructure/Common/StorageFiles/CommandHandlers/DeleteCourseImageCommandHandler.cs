using Gymphony.Application.Common.StorageFiles.Brokers;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.StorageFiles.CommandHandlers;

public class DeleteCourseImageCommandHandler(
    ICourseImageRepository courseImageRepository,
    IStorageFileRepository storageFileRepository,
    IAzureBlobStorageBroker azureBlobStorageBroker)
    : ICommandHandler<DeleteCourseImageCommand, bool>
{
    public async Task<bool> Handle(DeleteCourseImageCommand request, CancellationToken cancellationToken)
    {
        var foundImage = await courseImageRepository.Get(ci => ci.Id == request.CourseImageId)
            .Include(ci => ci.StorageFile)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Course image with id {request.CourseImageId} does not exist!");

        if (foundImage.StorageFile is null)
            throw new InvalidOperationException($"Course image with id {request.CourseImageId} does not contain storage file");

        var imagesCount = courseImageRepository
            .Get(ci => ci.CourseId == foundImage.CourseId, new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Count();

        if (imagesCount <= 1)
            throw new ArgumentException($"Course image with id {request.CourseImageId} is the only image of the course with id {foundImage.CourseId}. Upload a new image before deleting it!");

        await azureBlobStorageBroker.DeleteAsync(foundImage.StorageFile, cancellationToken);
        await storageFileRepository.DeleteAsync(foundImage.StorageFile, cancellationToken: cancellationToken);

        return true;
    }
}
