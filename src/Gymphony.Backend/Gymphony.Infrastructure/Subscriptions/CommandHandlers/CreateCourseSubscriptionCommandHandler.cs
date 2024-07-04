using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Subscriptions.CommandHandlers;

public class CreateCourseSubscriptionCommandHandler(IServiceProvider serviceProvider)
    : ICommandHandler<CreateCourseSubscriptionCommand, bool>
{
    public async Task<bool> Handle(CreateCourseSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var courseSubscriptionRepository =
            scope.ServiceProvider.GetRequiredService<ICourseSubscriptionRepository>();
            
        var courseSubscription = new CourseSubscription
        {
            MemberId = request.MemberId,
            CourseId = request.CourseId,
            LastSubscriptionPeriod = request.SubscriptionPeriod,
            StripeSubscriptionId = request.StripeSubscriptionId
        };

        await courseSubscriptionRepository.CreateAsync(courseSubscription, cancellationToken: cancellationToken);

        return true;
    }
}