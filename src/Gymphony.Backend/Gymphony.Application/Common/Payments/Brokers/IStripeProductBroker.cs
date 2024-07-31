using Gymphony.Application.Common.Payments.Models.Dtos;

namespace Gymphony.Application.Common.Payments.Brokers;

public interface IStripeProductBroker
{
    ValueTask<StripeProductDto> GetByIdAsync(string productId,
        CancellationToken cancellationToken = default);
    
    ValueTask<StripeProductDto> CreateAsync(StripeProductDto product,
        CancellationToken cancellationToken = default);

    ValueTask<StripeProductDto> UpdateAsync(StripeProductDto product, 
        CancellationToken cancellationToken = default);
}