using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class UnblockAdminCommandHandler(
    IEventBusBroker eventBusBroker,
    IRequestContextProvider requestContextProvider,
    IAdminRepository adminRepository) 
    : ICommandHandler<UnblockAdminCommand, bool>
{
    public async Task<bool> Handle(UnblockAdminCommand request, CancellationToken cancellationToken)
    {
        var actionAdminId = (Guid)requestContextProvider.GetUserIdFromClaims()!;

        if (actionAdminId == request.AdminId)
            return true;
        
        var foundAdmin = await adminRepository.GetByIdAsync(request.AdminId, cancellationToken: cancellationToken)
            ?? throw new EntityNotFoundException<Admin>($"Admin with id {request.AdminId} not found!");

        if (foundAdmin.Status is AccountStatus.Active or AccountStatus.Unverified)
            return true;
        
        foundAdmin.Status = AccountStatus.Unverified;
        await adminRepository.UpdateAsync(foundAdmin, cancellationToken: cancellationToken);

        await eventBusBroker.PublishLocalAsync(new AdminUnblockedEvent
        {
            UnblockedAdminId = foundAdmin.Id,
            UnblockedByAdminId = actionAdminId
        });

        return true;
    }
}