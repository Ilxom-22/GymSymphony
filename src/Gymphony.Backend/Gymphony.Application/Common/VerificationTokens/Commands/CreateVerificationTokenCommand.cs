using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.VerificationTokens.Commands;

public class CreateVerificationTokenCommand : ICommand<VerificationToken>
{
    public VerificationToken VerificationToken { get; set; } = default!;
}