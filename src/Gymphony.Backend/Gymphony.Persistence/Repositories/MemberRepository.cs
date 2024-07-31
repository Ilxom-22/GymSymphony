using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async ValueTask<Member?> GetByStripeCustomerIdAsync(string stripeCustomerId, 
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base.Get(member => member.StripeCustomerId == stripeCustomerId, queryOptions).FirstOrDefaultAsync(cancellationToken);
    }

    public new ValueTask<Member> CreateAsync(
        Member member, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(member, saveChanges, cancellationToken);
    }

    public new ValueTask<Member> UpdateAsync(Member member, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(member, saveChanges, cancellationToken);
    }
}