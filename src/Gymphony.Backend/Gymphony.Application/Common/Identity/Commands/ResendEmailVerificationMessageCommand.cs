using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Identity.Commands;

public class ResendEmailVerificationMessageCommand : ICommand<bool>
{
    public string EmailAddress { get; set; } = default!;
}