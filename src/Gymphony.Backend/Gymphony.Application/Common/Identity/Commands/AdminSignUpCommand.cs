using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Identity.Commands;

public class AdminSignUpCommand : ICommand<UserDto>
{
    public SignUpDetails SignUpDetails { get; set; } = default!;
}