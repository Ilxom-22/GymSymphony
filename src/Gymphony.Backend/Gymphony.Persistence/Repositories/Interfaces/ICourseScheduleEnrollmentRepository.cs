using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface ICourseScheduleEnrollmentRepository
{
    IQueryable<CourseScheduleEnrollment> Get(Expression<Func<CourseScheduleEnrollment, bool>>? predicate = default, QueryOptions queryOptions = default);

    ValueTask<CourseScheduleEnrollment> CreateAsync(CourseScheduleEnrollment courseScheduleEnrollment,
        bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<CourseScheduleEnrollment> DeleteAsync(CourseScheduleEnrollment courseScheduleEnrollment, 
        bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<int> BatchDeleteAsync(Expression<Func<CourseScheduleEnrollment, bool>> predicate, CancellationToken cancellationToken = default);
}
