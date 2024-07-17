using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface ICourseScheduleEnrollmentRepository
{
    ValueTask<CourseScheduleEnrollment> CreateAsync(CourseScheduleEnrollment courseScheduleEnrollment,
        bool saveChanges = true, CancellationToken cancellationToken = default);
}
