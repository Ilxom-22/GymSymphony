using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Persistence.Repositories;

public class CourseSubscriptionRepository(AppDbContext appDbContext)
    : EntityRepositoryBase<AppDbContext, CourseSubscription>(appDbContext),
        ICourseSubscriptionRepository
{
    public new IQueryable<CourseSubscription> Get(Expression<Func<CourseSubscription, bool>>? predicate = default, QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public new ValueTask<CourseSubscription> CreateAsync(CourseSubscription subscription, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(subscription, saveChanges, cancellationToken);
    }
}