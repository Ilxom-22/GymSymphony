using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class RemoveAdminCommandHandler(
    IEventBusBroker eventBusBroker,
    IRequestContextProvider requestContextProvider,
    IAdminRepository adminRepository) 
    : ICommandHandler<RemoveAdminCommand, bool>
{
    public async Task<bool> Handle(RemoveAdminCommand request, CancellationToken cancellationToken)
    {
        var actionAdminId = (Guid)requestContextProvider.GetUserIdFromClaims()!;

        if (actionAdminId == request.AdminId && adminRepository.GetActiveAdminsCount() < 2)
            throw new InvalidEntityStateChangeException<Admin>("You cannot remove yourself from admins since you are currently the only active administrator!");
        
        var foundAdmin = await adminRepository
            .GetByIdAsync(request.AdminId, cancellationToken: cancellationToken)
            ?? throw new EntityNotFoundException<Admin>($"Admin with id {request.AdminId} not found!");

        await adminRepository.DeleteAsync(foundAdmin, cancellationToken: cancellationToken);

        await eventBusBroker.PublishLocalAsync(new AdminRemovedEvent
        {
            RemovedAdmin = foundAdmin,
            RemovedByAdminId = actionAdminId
        });

        return true;
    }
}