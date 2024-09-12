using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class RemoveStaffCommandHandler(IStaffRepository staffRepository) 
    : ICommandHandler<RemoveStaffCommand, bool>
{
    public async Task<bool> Handle(RemoveStaffCommand request, CancellationToken cancellationToken)
    {
        var foundStaff = await staffRepository.Get(staff => staff.Id == request.StaffId)
            .Include(staff => staff.CourseSchedules)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Staff with id {request.StaffId} does not exist!");

        if (foundStaff.CourseSchedules is not null && foundStaff.CourseSchedules.Any())
            throw new ArgumentException($"{foundStaff.FirstName} {foundStaff.LastName} can't be removed as he has active courses!");

        await staffRepository.DeleteAsync(foundStaff, cancellationToken: cancellationToken);

        return true;
    }
}
