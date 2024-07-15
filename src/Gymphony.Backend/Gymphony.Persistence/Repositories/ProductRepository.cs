using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class ProductRepository(AppDbContext appDbContext) 
    : EntityRepositoryBase<AppDbContext, Product>(appDbContext), IProductRepository
{
    public new IQueryable<Product> Get(Expression<Func<Product, bool>>? predicate = default, QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public async ValueTask<Product?> GetProductByStripeProductId(string stripeProductId, QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base.Get(product => product.StripeDetails!.ProductId == stripeProductId, queryOptions)
            .Include(product => product.StripeDetails).FirstOrDefaultAsync(cancellationToken);
    }

    public new ValueTask<Product> UpdateAsync(Product product, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(product, saveChanges, cancellationToken);
    }
}