using AutoMapper;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.Common.Payments.CommandHandlers;

public class UpdateStripeDetailsCommandHandler(
    IMapper mapper,
    IStripeProductBroker stripeProductBroker,
    IStripePriceBroker stripePriceBroker)
    : ICommandHandler<UpdateStripeDetailsCommand, StripeDetails>
{
    public async Task<StripeDetails> Handle(UpdateStripeDetailsCommand request, CancellationToken cancellationToken)
    {
        var stripeProductDto = mapper.Map<StripeProductDto>(request.UpdatedProductDetails);
        stripeProductDto.Id = request.StripeDetails.ProductId;

        var product = await stripeProductBroker.UpdateAsync(stripeProductDto, cancellationToken);
        
        var oldPriceDetails = await stripePriceBroker
            .GetByIdAsync(request.StripeDetails.PriceId, cancellationToken);

        if (!PriceDetailsChanged(oldPriceDetails, request.UpdatedProductDetails))
            return new StripeDetails { ProductId = product.Id, PriceId = oldPriceDetails.Id };
        
        await stripePriceBroker.DeactivateAsync(oldPriceDetails.Id, cancellationToken);

        var stripePriceDto = mapper.Map<StripePriceDto>(request.UpdatedProductDetails);
        stripePriceDto.Product = product.Id;

        var price = await stripePriceBroker.CreateAsync(stripePriceDto, cancellationToken);

        return new StripeDetails { ProductId = product.Id, PriceId = price.Id };
    }

    private bool PriceDetailsChanged(StripePriceDto oldPrice, StripeProductDetails productDetails)
    {
        if (oldPrice.UnitAmountDecimal != productDetails.Price * 100)
            return true;

        if (oldPrice.Recurring?.Interval != productDetails.DurationUnit.ToLowerString())
            return true;

        return oldPrice.Recurring?.IntervalCount != productDetails.DurationCount;
    }
}