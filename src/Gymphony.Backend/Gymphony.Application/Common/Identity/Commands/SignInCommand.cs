using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Identity.Commands;

public class SignInCommand : ICommand<IdentityTokenDto>
{
    public SignInDetails SignInDetails { get; set; } = default!;

    public Provider AuthProvider { get; set; }
}