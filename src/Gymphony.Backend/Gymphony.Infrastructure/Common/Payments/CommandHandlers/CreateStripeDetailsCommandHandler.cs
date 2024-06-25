using AutoMapper;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Infrastructure.Common.Payments.CommandHandlers;

public class CreateStripeDetailsCommandHandler(
    IMapper mapper,
    IStripeProductBroker stripeProductBroker, 
    IStripePriceBroker stripePriceBroker) 
    : ICommandHandler<CreateStripeDetailsCommand, StripeDetails>
{
    public async Task<StripeDetails> Handle(CreateStripeDetailsCommand request, CancellationToken cancellationToken)
    {
        var stripeProductDto = mapper.Map<StripeProductDto>(request.ProductDetails);
        var product = await stripeProductBroker.CreateAsync(stripeProductDto, cancellationToken);

        var stripePriceDto = mapper.Map<StripePriceDto>(request.ProductDetails);
        stripePriceDto.Product = product.Id;

        var price = await stripePriceBroker.CreateAsync(stripePriceDto, cancellationToken);

        return new StripeDetails { ProductId = product.Id, PriceId = price.Id };
    }
}