namespace Gymphony.Application.Common.Identity.Models.Dtos;

public class IdentityTokenDto
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}