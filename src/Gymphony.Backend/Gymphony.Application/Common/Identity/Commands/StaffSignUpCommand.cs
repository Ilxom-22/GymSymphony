using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Microsoft.AspNetCore.Http;

namespace Gymphony.Application.Common.Identity.Commands;

public class StaffSignUpCommand : ICommand<StaffDto>
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Bio { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public IFormFile ProfileImage { get; set; } = default!;
}