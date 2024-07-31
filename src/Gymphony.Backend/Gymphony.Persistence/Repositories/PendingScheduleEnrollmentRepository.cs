using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class PendingScheduleEnrollmentRepository(AppDbContext dbContext) : EntityRepositoryBase<AppDbContext, PendingScheduleEnrollment>(dbContext),
    IPendingScheduleEnrollmentRepository
{
    public async ValueTask<List<PendingScheduleEnrollment>> GetByCheckoutSessionIdAsync(string checkoutSessionId, 
        QueryOptions queryOptions = default, CancellationToken cancellationToken = default)
    {
        return await base.Get(pse => pse.StripeSessionId == checkoutSessionId, queryOptions)
            .ToListAsync(cancellationToken);
    }

    public new ValueTask<PendingScheduleEnrollment> DeleteAsync(PendingScheduleEnrollment pendingScheduleEnrollment, 
        bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(pendingScheduleEnrollment, saveChanges, cancellationToken);
    }

    public ValueTask<int> BatchDeleteBySessionIdAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return base.BatchDeleteAsync(pse => pse.StripeSessionId == sessionId, cancellationToken);
    }
}
