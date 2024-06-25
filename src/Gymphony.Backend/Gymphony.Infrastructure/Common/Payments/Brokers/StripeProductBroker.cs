using AutoMapper;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Infrastructure.Common.Payments.Services;
using Stripe;

namespace Gymphony.Infrastructure.Common.Payments.Brokers;

public class StripeProductBroker(IMapper mapper, StripeProductService stripeProductService) 
    : IStripeProductBroker
{
    public async ValueTask<StripeProductDto> GetByIdAsync(string productId,
        CancellationToken cancellationToken = default)
    {
        var stripeProduct = await stripeProductService.GetAsync(productId, cancellationToken: cancellationToken);
        
        return mapper.Map<StripeProductDto>(stripeProduct);
    }

    public async ValueTask<StripeProductDto> CreateAsync(StripeProductDto product, 
        CancellationToken cancellationToken = default)
    {
        var productCreateOptions = mapper.Map<ProductCreateOptions>(product);

        var stripeProduct = await stripeProductService
            .CreateAsync(productCreateOptions, cancellationToken: cancellationToken);

        return mapper.Map<StripeProductDto>(stripeProduct);
    }

    public async ValueTask<StripeProductDto> UpdateAsync(StripeProductDto product,
        CancellationToken cancellationToken = default)
    {
        var productUpdateOptions = mapper.Map<ProductUpdateOptions>(product);

        var stripeProduct = await stripeProductService
            .UpdateAsync(product.Id, productUpdateOptions, cancellationToken: cancellationToken);
        
        return mapper.Map<StripeProductDto>(stripeProduct);
    }
}