using Force.DeepCloner;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Infrastructure.Common.Payments.CommandHandlers;

public class UpdateStripePriceCommandHandler(IStripePriceBroker stripePriceBroker) : ICommandHandler<UpdateStripePriceCommand, string>
{
    public async Task<string> Handle(UpdateStripePriceCommand request, CancellationToken cancellationToken)
    {
        var oldPrice = await stripePriceBroker.GetByIdAsync(request.StripeDetails.PriceId, cancellationToken);

        if (oldPrice.UnitAmountDecimal == request.NewPrice * 100)
            return oldPrice.Id;

        var newPrice = oldPrice.DeepClone();
        newPrice.UnitAmountDecimal = request.NewPrice;
        newPrice.Id = null!;
        newPrice.Product = request.StripeDetails.ProductId;

        await stripePriceBroker.DeactivateAsync(oldPrice.Id, cancellationToken);
        newPrice = await stripePriceBroker.CreateAsync(newPrice, cancellationToken);
        
        return newPrice.Id;
    }
}