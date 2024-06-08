namespace Gymphony.Application.Identity.Brokers;

public interface IRequestContextProvider
{
    public Guid? GetUserId();
}