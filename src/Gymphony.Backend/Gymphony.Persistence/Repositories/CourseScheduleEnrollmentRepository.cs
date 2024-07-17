using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories;

public class CourseScheduleEnrollmentRepository(AppDbContext dbContext) : EntityRepositoryBase<AppDbContext, CourseScheduleEnrollment>(dbContext),
    ICourseScheduleEnrollmentRepository
{
    public new IQueryable<CourseScheduleEnrollment> Get(Expression<Func<CourseScheduleEnrollment, bool>>? predicate = default, 
        QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public new ValueTask<CourseScheduleEnrollment> CreateAsync(CourseScheduleEnrollment courseScheduleEnrollment,
        bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(courseScheduleEnrollment, saveChanges, cancellationToken);
    }

    public new ValueTask<CourseScheduleEnrollment> DeleteAsync(CourseScheduleEnrollment courseScheduleEnrollment, 
        bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(courseScheduleEnrollment, saveChanges, cancellationToken);
    }

    public new ValueTask<int> BatchDeleteAsync(Expression<Func<CourseScheduleEnrollment, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return base.BatchDeleteAsync(predicate, cancellationToken);
    }
}
