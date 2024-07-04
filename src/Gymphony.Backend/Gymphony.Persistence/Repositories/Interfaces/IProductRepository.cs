using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IProductRepository
{
    ValueTask<Product?> GetProductByStripeProductId(string stripeProductId, 
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);
}