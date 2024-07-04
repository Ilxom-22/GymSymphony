using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface ICourseSubscriptionRepository
{
    IQueryable<CourseSubscription> Get(Expression<Func<CourseSubscription, bool>>? predicate = default,
        QueryOptions queryOptions = default);
    
    ValueTask<CourseSubscription> CreateAsync(CourseSubscription subscription, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}