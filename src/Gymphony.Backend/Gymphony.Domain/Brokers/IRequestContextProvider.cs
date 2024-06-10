namespace Gymphony.Domain.Brokers;

public interface IRequestContextProvider
{
    public Guid? GetUserId();
}