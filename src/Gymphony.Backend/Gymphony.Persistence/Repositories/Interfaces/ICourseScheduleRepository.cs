using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface ICourseScheduleRepository
{
    IQueryable<CourseSchedule> Get(Expression<Func<CourseSchedule, bool>>? predicate = default, QueryOptions queryOptions = default);

    ValueTask<IList<CourseSchedule>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken = default);

    ValueTask<CourseSchedule?> GetByIdAsync(Guid scheduleId, QueryOptions queryOptions = default, CancellationToken cancellationToken = default);

    ValueTask<CourseSchedule> DeleteAsync(CourseSchedule courseSchedule, bool saveChanges = true, CancellationToken cancellationToken = default);
}
