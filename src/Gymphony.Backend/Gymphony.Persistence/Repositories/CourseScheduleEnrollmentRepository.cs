using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Persistence.Repositories;

public class CourseScheduleEnrollmentRepository(AppDbContext dbContext) : EntityRepositoryBase<AppDbContext, CourseScheduleEnrollment>(dbContext),
    ICourseScheduleEnrollmentRepository
{
    public new ValueTask<CourseScheduleEnrollment> CreateAsync(CourseScheduleEnrollment courseScheduleEnrollment,
        bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(courseScheduleEnrollment, saveChanges, cancellationToken);
    }
}
