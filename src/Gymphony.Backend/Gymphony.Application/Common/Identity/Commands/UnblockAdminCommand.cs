using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Identity.Commands;

public class UnblockAdminCommand : ICommand<bool>
{
    public Guid AdminId { get; set; }
}