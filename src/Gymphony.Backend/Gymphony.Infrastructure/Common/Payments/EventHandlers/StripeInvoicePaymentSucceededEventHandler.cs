using AutoMapper;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Events;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Common.Payments.EventHandlers;

public class StripeInvoicePaymentSucceededEventHandler(
    IServiceProvider serviceProvider,
    IMapper mapper,
    IMediator mediator) 
    : IEventHandler<StripeInvoicePaymentSucceededEvent>
{
    public async Task Handle(StripeInvoicePaymentSucceededEvent notification, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var memberRepository = scope.ServiceProvider.GetRequiredService<IMemberRepository>();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var stripeSubscriptionBroker = scope.ServiceProvider.GetRequiredService<IStripeSubscriptionBroker>();

        var stripeSubscription = await stripeSubscriptionBroker.GetByIdAsync(notification.Invoice.SubscriptionId, cancellationToken)
            ?? throw new InvalidOperationException($"Subscription with id {notification.Invoice.SubscriptionId} not found!");

        var subscriptionDto = mapper.Map<StripeSubscriptionDto>(stripeSubscription);

        var customer = await memberRepository
                           .GetByStripeCustomerIdAsync(notification.Invoice.CustomerId, new QueryOptions(QueryTrackingMode.AsNoTracking), cancellationToken)
                       ?? throw new InvalidOperationException($"Member with stripe customer id {notification.Invoice.CustomerId} not found!");

        var product = await productRepository.GetProductByStripeProductId(subscriptionDto.ProductId,
                          new QueryOptions(QueryTrackingMode.AsNoTracking), cancellationToken)
                      ?? throw new InvalidOperationException($"Product with stripe product id {subscriptionDto.ProductId} does not exist!");

        var command = mapper.Map<CreateOrRenewSubscriptionCommand>(notification.Invoice);
        mapper.Map(subscriptionDto, command);

        command.MemberId = customer.Id;
        command.ProductId = product.Id;

        await mediator.Send(command, cancellationToken);
    }
}