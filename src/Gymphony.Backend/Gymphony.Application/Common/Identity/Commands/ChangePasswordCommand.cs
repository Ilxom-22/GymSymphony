using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Identity.Commands;

public class ChangePasswordCommand : ICommand<bool>
{
    public string OldPassword { get; set; } = default!;

    public string NewPassword { get; set; } = default!;
}