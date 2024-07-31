using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IPendingScheduleEnrollmentRepository
{
    ValueTask<List<PendingScheduleEnrollment>> GetByCheckoutSessionIdAsync(string checkoutSessionId, QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);

    ValueTask<PendingScheduleEnrollment> DeleteAsync(PendingScheduleEnrollment pendingScheduleEnrollment, 
        bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<int> BatchDeleteBySessionIdAsync(string sessionId, CancellationToken cancellationToken = default);
}
