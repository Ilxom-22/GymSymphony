using Gymphony.Domain.Common.Entities;

namespace Gymphony.Application.Common.Exceptions;

public abstract class InvalidEntityStateChangeException(string message) : Exception(message)
{
    
}

public class InvalidEntityStateChangeException<TEntity>(string message) 
    : InvalidEntityStateChangeException(message)
    where TEntity : IEntity
{
    
}