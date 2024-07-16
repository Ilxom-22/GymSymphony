using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Identity.Commands;

public class RemoveStaffCommand : ICommand<bool>
{
    public Guid StaffId { get; set; }
}
