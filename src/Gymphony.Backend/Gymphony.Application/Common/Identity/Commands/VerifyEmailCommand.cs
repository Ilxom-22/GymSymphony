using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Identity.Commands;

public class VerifyEmailCommand : ICommand<bool>
{
    public string Token { get; set; } = default!;
}