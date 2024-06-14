using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.Identity.Commands;

public class RefreshTokenCommand : ICommand<IdentityTokenDto>
{
    public string RefreshToken { get; set; } = default!;
}