using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IMemberRepository
{
    IQueryable<Member> Get(
        Expression<Func<Member, bool>>? predicate = default,
        QueryOptions queryOptions = default);
    
    ValueTask<Member> CreateAsync(
        Member member, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}