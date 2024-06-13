using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Identity.Commands;

public class MemberSignUpCommand : ICommand<UserDto>
{
    public SignUpDetails SignUpDetails { get; set; } = default!;

    public Provider AuthProvider { get; set; }
}