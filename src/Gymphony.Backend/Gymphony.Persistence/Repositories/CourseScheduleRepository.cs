using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories;

public class CourseScheduleRepository(AppDbContext dbContext)
    : EntityRepositoryBase<AppDbContext, CourseSchedule>(dbContext), ICourseScheduleRepository
{
    public new IQueryable<CourseSchedule> Get(Expression<Func<CourseSchedule, bool>>? predicate = default, QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public async ValueTask<IList<CourseSchedule>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await base.Get()
            .Where(schedule => ids.Contains(schedule.Id))
            .ToListAsync(cancellationToken);
    }

    public async ValueTask<CourseSchedule?> GetByIdAsync(Guid scheduleId, QueryOptions queryOptions = default, 
        CancellationToken cancellationToken = default)
    {
        return await base.Get(schedule => schedule.Id == scheduleId, queryOptions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public new ValueTask<CourseSchedule> UpdateAsync(CourseSchedule courseSchedule, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(courseSchedule, saveChanges, cancellationToken);
    }

    public new ValueTask<CourseSchedule> DeleteAsync(CourseSchedule courseSchedule, bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(courseSchedule, saveChanges, cancellationToken);
    }
}
