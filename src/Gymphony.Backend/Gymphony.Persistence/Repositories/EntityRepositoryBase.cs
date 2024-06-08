using System.Linq.Expressions;
using Gymphony.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public abstract class EntityRepositoryBase<TContext, TEntity>(TContext context)
    where TContext : DbContext 
    where TEntity : class, IEntity
{
    public IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? predicate = default, 
        bool asNoTracking = false)
    {
        var query = context.Set<TEntity>().AsQueryable();

        if (predicate is not null)
            query = query.Where(predicate);

        return asNoTracking ? query.AsNoTracking() : query;
    }

    protected async ValueTask<TEntity> CreateAsync(
        TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        
        await SaveChangesIfRequested(saveChanges, cancellationToken);

        return entity;
    }

    protected async ValueTask<TEntity> UpdateAsync(TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().Update(entity);

        await SaveChangesIfRequested(saveChanges, cancellationToken);

        return entity;
    }

    protected async ValueTask<TEntity> DeleteAsync(
        TEntity entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().Remove(entity);

        await SaveChangesIfRequested(saveChanges, cancellationToken);

        return entity;
    }
    
    private async ValueTask SaveChangesIfRequested(
        bool saveChanges,
        CancellationToken cancellationToken = default)
    {
        if (saveChanges && context.ChangeTracker.HasChanges())
            await context.SaveChangesAsync(cancellationToken);
    }
}