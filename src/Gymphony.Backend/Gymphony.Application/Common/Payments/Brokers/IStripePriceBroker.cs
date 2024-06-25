using Gymphony.Application.Common.Payments.Models.Dtos;

namespace Gymphony.Application.Common.Payments.Brokers;

public interface IStripePriceBroker
{
    ValueTask<StripePriceDto> GetByIdAsync(string priceId, 
        CancellationToken cancellationToken = default);
    
    ValueTask<StripePriceDto> CreateAsync(StripePriceDto price, 
        CancellationToken cancellationToken = default);

    ValueTask<StripePriceDto> DeactivateAsync(string priceId,
        CancellationToken cancellationToken = default);
}