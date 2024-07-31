using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories;

public class CourseRepository(AppDbContext dbContext) : EntityRepositoryBase<AppDbContext, Course>(dbContext), ICourseRepository
{
    public new IQueryable<Course> Get(Expression<Func<Course, bool>>? predicate = default, QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public async ValueTask<Course?> GetByIdAsync(Guid courseId, QueryOptions queryOptoins = default, CancellationToken cancellationToken = default)
    {
        return await base
            .Get(course => course.Id == courseId, queryOptoins)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async ValueTask<bool> CourseExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await base
            .Get(queryOptions: new QueryOptions(QueryTrackingMode.AsNoTracking))
            .AnyAsync(course => course.Name == name, cancellationToken);
    }

    public new ValueTask<Course> CreateAsync(Course course, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(course, saveChanges, cancellationToken);
    }

    public new ValueTask<Course> UpdateAsync(Course course, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(course, saveChanges, cancellationToken);
    }

    public new ValueTask<Course> DeleteAsync(Course course, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(course, saveChanges, cancellationToken);
    }
}
