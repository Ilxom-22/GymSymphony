using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Subscriptions.CommandHandlers;

public class CreateCourseSubscriptionCommandHandler(IServiceProvider serviceProvider)
    : ICommandHandler<CreateCourseSubscriptionCommand, CourseSubscription>
{
    public async Task<CourseSubscription> Handle(CreateCourseSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var courseSubscriptionRepository =
            scope.ServiceProvider.GetRequiredService<ICourseSubscriptionRepository>();
            
        var courseSubscription = new CourseSubscription
        {
            MemberId = request.MemberId,
            CourseId = request.CourseId,
            StripeSubscriptionId = request.StripeSubscriptionId
        };

        await courseSubscriptionRepository.CreateAsync(courseSubscription, cancellationToken: cancellationToken);

        request.SubscriptionPeriod.SubscriptionId = courseSubscription.Id;
        courseSubscription.LastSubscriptionPeriod = request.SubscriptionPeriod;

        await courseSubscriptionRepository.UpdateAsync(courseSubscription,
            cancellationToken: cancellationToken);

        return courseSubscription;
    }
}