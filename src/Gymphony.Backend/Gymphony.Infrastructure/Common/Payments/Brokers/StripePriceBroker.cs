using AutoMapper;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Infrastructure.Common.Payments.Services;
using Stripe;

namespace Gymphony.Infrastructure.Common.Payments.Brokers;

public class StripePriceBroker(IMapper mapper, StripePriceService stripePriceService) 
    : IStripePriceBroker
{
    public async ValueTask<StripePriceDto> GetByIdAsync(string priceId, CancellationToken cancellationToken = default)
    {
        var stripePrice = await stripePriceService
            .GetAsync(priceId, cancellationToken: cancellationToken);

        return mapper.Map<StripePriceDto>(stripePrice);
    }

    public async ValueTask<StripePriceDto> CreateAsync(StripePriceDto price, CancellationToken cancellationToken = default)
    {
        var priceCreateOptions = mapper.Map<PriceCreateOptions>(price);
        priceCreateOptions.UnitAmountDecimal *= 100;

        var stripePrice = await stripePriceService
            .CreateAsync(priceCreateOptions, cancellationToken: cancellationToken);

        return mapper.Map<StripePriceDto>(stripePrice);
    }

    public async ValueTask<StripePriceDto> DeactivateAsync(string priceId,
        CancellationToken cancellationToken = default)
    {
        var priceUpdateOptions = new PriceUpdateOptions { Active = false };

        var price = await stripePriceService
            .UpdateAsync(priceId, priceUpdateOptions, cancellationToken: cancellationToken);

        return mapper.Map<StripePriceDto>(price);
    }
}