using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Persistence.Repositories;

public class MemberRepository(AppDbContext dbContext) :
    EntityRepositoryBase<AppDbContext, Member>(dbContext), 
    IMemberRepository
{
    public new IQueryable<Member> Get(
        Expression<Func<Member, bool>>? predicate = default,
        QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public new ValueTask<Member> CreateAsync(
        Member member, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(member, saveChanges, cancellationToken);
    }
}