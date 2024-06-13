namespace Gymphony.Domain.Brokers;

public interface IRequestContextProvider
{
    Guid? GetUserIdFromClaims();

    Guid? GetUserIdFromClaimsOrToken();

    string? GetAccessToken();
}