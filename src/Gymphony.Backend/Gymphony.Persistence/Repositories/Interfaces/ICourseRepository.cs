using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface ICourseRepository
{
    IQueryable<Course> Get(Expression<Func<Course, bool>>? predicate = default, QueryOptions queryOptions = default);

    ValueTask<Course?> GetByIdAsync(Guid courseId, QueryOptions queryOptoins = default, CancellationToken cancellationToken = default);

    ValueTask<bool> CourseExistsAsync(string name, CancellationToken cancellationToken = default);

    ValueTask<Course> CreateAsync(Course course, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Course> UpdateAsync(Course course, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Course> DeleteAsync(Course course, bool saveChanges = true, CancellationToken cancellationToken = default);
}
