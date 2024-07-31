using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.StorageFiles.Commands;

public class DeleteCourseImageCommand : ICommand<bool>
{
    public Guid CourseImageId { get; set; }
}
