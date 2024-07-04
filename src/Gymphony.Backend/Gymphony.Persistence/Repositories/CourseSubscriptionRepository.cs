using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Persistence.Repositories;

public class CourseSubscriptionRepository(AppDbContext appDbContext)
    : EntityRepositoryBase<AppDbContext, CourseSubscription>(appDbContext),
        ICourseSubscriptionRepository
{
    public new ValueTask<CourseSubscription> CreateAsync(CourseSubscription subscription, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(subscription, saveChanges, cancellationToken);
    }
}