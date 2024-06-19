using Gymphony.Application.Common.Notifications.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Common.Notifications.CommandHandlers;

public class GetNotificationTemplateByTypeCommandHandler(
    IServiceProvider serviceProvider) 
    : ICommandHandler<GetNotificationTemplateByTypeCommand, NotificationTemplate?>
{
    public async Task<NotificationTemplate?> Handle(GetNotificationTemplateByTypeCommand request, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var notificationTemplateRepository = scope.ServiceProvider
            .GetRequiredService<INotificationTemplateRepository>();
        
        var foundTemplate = await notificationTemplateRepository
                .GetByType(request.TemplateType, request.QueryOptions, cancellationToken);

        return foundTemplate;
    }
}