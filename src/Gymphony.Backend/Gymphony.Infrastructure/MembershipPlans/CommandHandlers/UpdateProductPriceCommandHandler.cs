using AutoMapper;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Events;
using Gymphony.Application.Products.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.MembershipPlans.CommandHandlers;

public class UpdateProductPriceCommandHandler(
    IMapper mapper, IMediator mediator,
    IProductRepository productRepository,
    IEventBusBroker eventBusBroker) 
    : ICommandHandler<UpdateProductPriceCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
    {
        var foundProduct = await productRepository
            .Get(product => product.Id == request.ProductId)
            .Include(product => product.StripeDetails)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ArgumentException($"Product with id '{request.ProductId}' does not exist!");

        if (foundProduct.StripeDetails is null)
            throw new InvalidOperationException();

        if (foundProduct.Status is ContentStatus.DeactivationRequested or ContentStatus.Deactivated)
            throw new ArgumentException(
                "Updating the price is not allowed for products that are deactivated or have a deactivation request in progress!");

        if (request.Price < 1)
            throw new ArgumentException("Price must be greater than 1");

        var newPriceId =
            await mediator.Send(
                new UpdateStripePriceCommand 
                    { StripeDetails = foundProduct.StripeDetails, NewPrice = request.Price },
                cancellationToken);

        foundProduct.Price = request.Price;
        foundProduct.StripeDetails.PriceId = newPriceId;
        
        await productRepository.UpdateAsync(foundProduct, cancellationToken: cancellationToken);

        if (foundProduct.Status is ContentStatus.Activated)
            await eventBusBroker.PublishLocalAsync(new ProductPriceUpdatedEvent
                { Product = foundProduct }, cancellationToken);

        return mapper.Map<ProductDto>(foundProduct);
    }
}