using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IProductRepository
{
    IQueryable<Product> Get(Expression<Func<Product, bool>>? predicate = default,
        QueryOptions queryOptions = default);
    
    ValueTask<Product?> GetProductByStripeProductId(string stripeProductId, 
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);

    ValueTask<Product> UpdateAsync(Product product, 
        bool saveChanges = true, CancellationToken cancellationToken = default);
}