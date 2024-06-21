using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Identity.Commands;

public class ResetPasswordCommand : ICommand<bool>
{
    public string Token { get; set; } = default!;

    public string NewPassword { get; set; } = default!;
}