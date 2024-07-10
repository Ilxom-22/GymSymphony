using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IStaffRepository
{
    IQueryable<Staff> Get(Expression<Func<Staff, bool>>? predicate = default, QueryOptions queryOptions = default);

    ValueTask<Staff> CreateAsync(Staff staff, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Staff> DeleteAsync(Staff staff, bool saveChanges = true, CancellationToken cancellationToken = default);
}
