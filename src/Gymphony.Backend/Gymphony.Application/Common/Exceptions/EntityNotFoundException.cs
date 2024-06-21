using Gymphony.Domain.Common.Entities;

namespace Gymphony.Application.Common.Exceptions;

public abstract class EntityNotFoundException(string message) : Exception(message)
{
    
}

public class EntityNotFoundException<TEntity>(string message) : EntityNotFoundException(message)
    where TEntity : IEntity
{
    
}