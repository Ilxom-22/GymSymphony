namespace Gymphony.Application.Common.Identity.Models.Dtos;

public class SignInDetails
{
    public string EmailAddress { get; set; } = default!;

    public string AuthData { get; set; } = default!;
}