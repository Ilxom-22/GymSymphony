using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class ProductRepository(AppDbContext appDbContext) 
    : EntityRepositoryBase<AppDbContext, Product>(appDbContext), IProductRepository
{
    public async ValueTask<Product?> GetProductByStripeProductId(string stripeProductId, QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base.Get(product => product.StripeDetails!.ProductId == stripeProductId, queryOptions)
            .Include(product => product.StripeDetails).FirstOrDefaultAsync(cancellationToken);
    }
}