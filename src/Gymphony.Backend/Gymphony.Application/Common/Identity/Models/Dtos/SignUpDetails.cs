namespace Gymphony.Application.Common.Identity.Models.Dtos;

public class SignUpDetails
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public string AuthData { get; set; } = default!;

    public DateTimeOffset? BirthDay { get; set; }
}