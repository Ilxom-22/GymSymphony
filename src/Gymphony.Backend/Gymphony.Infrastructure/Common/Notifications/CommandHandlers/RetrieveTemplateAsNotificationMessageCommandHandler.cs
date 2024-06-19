using AutoMapper;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Notifications.Commands;
using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using MediatR;

namespace Gymphony.Infrastructure.Common.Notifications.CommandHandlers;

public class RetrieveTemplateAsNotificationMessageCommandHandler(
    IMediator mediator, IMapper mapper)
    : ICommandHandler<RetrieveTemplateAsNotificationMessageCommand, NotificationMessage>
{
    public async Task<NotificationMessage> Handle(RetrieveTemplateAsNotificationMessageCommand request, CancellationToken cancellationToken)
    {
        var notificationTemplate = await mediator.Send(new GetNotificationTemplateByTypeCommand
                                   {
                                       TemplateType = request.TemplateType, 
                                       QueryOptions = new QueryOptions(QueryTrackingMode.AsNoTracking)
                                   }, cancellationToken)
            ?? throw new EntityNotFoundException<NotificationTemplate>($"Requested notification template { request.TemplateType.ToString() } not found!");

        return mapper.Map<NotificationMessage>(notificationTemplate);
    }
}