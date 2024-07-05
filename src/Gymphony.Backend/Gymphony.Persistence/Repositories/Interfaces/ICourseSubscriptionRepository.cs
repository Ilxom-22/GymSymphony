using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface ICourseSubscriptionRepository
{
    ValueTask<CourseSubscription> CreateAsync(CourseSubscription subscription, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}