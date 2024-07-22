using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories;

public class CourseImageRepository(AppDbContext dbContext) : EntityRepositoryBase<AppDbContext, CourseImage>(dbContext), ICourseImageRepository
{
    public new IQueryable<CourseImage> Get(Expression<Func<CourseImage, bool>>? predicate = default, QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public async ValueTask<CourseImage?> GetByIdAsync(Guid courseImageId, QueryOptions queryOptions = default, 
        CancellationToken cancellationToken = default)
    {
        return await base.Get(ci => ci.Id == courseImageId, queryOptions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public new ValueTask<CourseImage> CreateAsync(CourseImage courseImage, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(courseImage, saveChanges, cancellationToken);
    }

    public new ValueTask<CourseImage> DeleteAsync(CourseImage courseImage, bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(courseImage, saveChanges, cancellationToken);
    }
}
