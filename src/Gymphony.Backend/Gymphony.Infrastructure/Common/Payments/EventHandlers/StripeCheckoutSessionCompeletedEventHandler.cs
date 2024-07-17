using AutoMapper;
using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.Common.Payments.Events;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Common.Payments.EventHandlers;

public class StripeCheckoutSessionCompeletedEventHandler(IServiceProvider serviceProvider, IMediator mediator, IMapper mapper) 
    : IEventHandler<StripeCheckoutSessionCompeletedEvent>
{
    public async Task Handle(StripeCheckoutSessionCompeletedEvent notification, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var memberRepository = scope.ServiceProvider.GetRequiredService<IMemberRepository>();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var pendingScheduleEnrollmentRepository = scope.ServiceProvider
            .GetRequiredService<IPendingScheduleEnrollmentRepository>();
        var courseScheduleEnrollmentRepository = scope.ServiceProvider.GetRequiredService<ICourseScheduleEnrollmentRepository>();
        var stripeSubscriptionBroker = scope.ServiceProvider.GetRequiredService<IStripeSubscriptionBroker>();

        var stripeSubscription = await stripeSubscriptionBroker.GetByIdAsync(notification.SubscriptionId, cancellationToken)
            ?? throw new InvalidOperationException($"Subscription with id {notification.SubscriptionId} not found!");

        var subscriptionDto = mapper.Map<StripeSubscriptionDto>(stripeSubscription);

        var customer = await memberRepository
                           .GetByStripeCustomerIdAsync(notification.CustomerId, new QueryOptions(QueryTrackingMode.AsNoTracking), cancellationToken)
                       ?? throw new InvalidOperationException($"Member with stripe customer id {notification.CustomerId} not found!");

        var product = await productRepository.GetProductByStripeProductId(subscriptionDto.ProductId,
                          new QueryOptions(QueryTrackingMode.AsNoTracking), cancellationToken)
                      ?? throw new InvalidOperationException($"Product with stripe product id {subscriptionDto.ProductId} does not exist!");

        if (product.Type == ProductType.MembershipPlan)
            return;

        var subscription = await mediator.Send(new CreateCourseSubscriptionCommand 
        {
            CourseId = product.Id,
            MemberId = customer.Id,
            StripeSubscriptionId = subscriptionDto.Id,
            SubscriptionPeriod = new SubscriptionPeriod
            {
                StartDate = DateOnly.FromDateTime(subscriptionDto.SubscriptionStartDate),
                ExpiryDate = DateOnly.FromDateTime(subscriptionDto.SubscriptionEndDate),
                Payment = new Payment
                {
                    Amount = notification.PaymentAmount,
                    MemberId = customer.Id,
                    Date = notification.PaymentDate
                }
            }
        });

        var pendingEnrollments = await pendingScheduleEnrollmentRepository.GetByCheckoutSessionIdAsync(notification.SessionId, cancellationToken: cancellationToken);


        foreach (var enrollment in pendingEnrollments)
        {
            var courseScheduleEnrollment = new CourseScheduleEnrollment
            {
                CourseScheduleId = enrollment.CourseScheduleId,
                CourseSubscriptionId = subscription.Id,
                MemberId = subscription.MemberId,
                EnrollmentDate = DateTime.UtcNow
            };

            await courseScheduleEnrollmentRepository.CreateAsync(courseScheduleEnrollment, cancellationToken: cancellationToken);

            await pendingScheduleEnrollmentRepository.DeleteAsync(enrollment, cancellationToken: cancellationToken);
        }
    }
}
