using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface ICourseImageRepository
{
    IQueryable<CourseImage> Get(Expression<Func<CourseImage, bool>>? predicate = default, QueryOptions queryOptions = default);

    ValueTask<CourseImage?> GetByIdAsync(Guid courseImageId, QueryOptions queryOptions = default, CancellationToken cancellationToken = default);

    ValueTask<CourseImage> CreateAsync(CourseImage courseImage, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<CourseImage> DeleteAsync(CourseImage courseImage, bool saveChanges = true, CancellationToken cancellationToken = default);
}
