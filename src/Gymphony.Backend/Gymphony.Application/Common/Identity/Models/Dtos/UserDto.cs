using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Identity.Models.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public Role Role { get; set; }

    public string EmailAddress { get; set; } = default!;
}