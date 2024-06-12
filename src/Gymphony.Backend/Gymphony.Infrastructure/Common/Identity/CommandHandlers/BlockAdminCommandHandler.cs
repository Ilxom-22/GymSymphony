using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class BlockAdminCommandHandler(
    IRequestContextProvider requestContextProvider,
    IAdminRepository adminRepository)
    : ICommandHandler<BlockAdminCommand, bool>
{
    public async Task<bool> Handle(BlockAdminCommand request, CancellationToken cancellationToken)
    {
        var actionAdminId = requestContextProvider.GetUserId()!;
        
        if (actionAdminId == request.AdminId && adminRepository.GetActiveAdminsCount() < 2)
            throw new InvalidEntityStateChangeException<Admin>(
                "You cannot block yourself since you are currently the only active administrator!");
        
        var foundAdmin = await adminRepository
            .Get(admin => admin.Id == request.AdminId)
            .Include(admin => admin.AccessToken)
            .Include(admin => admin.RefreshToken)
            .FirstOrDefaultAsync(cancellationToken)
                         ?? throw new EntityNotFoundException<Admin>($"Admin with id {request.AdminId} not found!");

        foundAdmin.Status = AccountStatus.Blocked;
        foundAdmin.AccessToken = null;
        foundAdmin.RefreshToken = null;
        
        await adminRepository.UpdateAsync(foundAdmin, cancellationToken: cancellationToken);

        return true;
    }
}