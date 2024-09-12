using System.Linq.Expressions;
using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Gymphony.Persistence.Repositories;

public abstract class EntityRepositoryBase<TContext, TEntity>(TContext context)
    where TContext : DbContext 
    where TEntity : class, IEntity
{
    private IDbContextTransaction? _currentTransaction;

    public async ValueTask BeginTransactionAsync()
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _currentTransaction = await context.Database.BeginTransactionAsync();
    }

    public async ValueTask CommitTransactionAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No transaction in progress.");

        try
        {
            await context.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async ValueTask RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    protected IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? predicate = default, 
        QueryOptions queryOptions = default)
    {
        var query = context.Set<TEntity>().AsQueryable();

        if (predicate is not null)
            query = query.Where(predicate);

        return query.ApplyQueryOptions(queryOptions);
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

    protected async ValueTask<int> BatchDeleteAsync(
        Expression<Func<TEntity, bool>> batchDeletePredicate,
        CancellationToken cancellationToken = default)
    {
        var entities = context.Set<TEntity>().Where(batchDeletePredicate);

        return await entities.ExecuteDeleteAsync(cancellationToken);
    }

    private async ValueTask SaveChangesIfRequested(
        bool saveChanges,
        CancellationToken cancellationToken = default)
    {
        if (saveChanges && context.ChangeTracker.HasChanges())
            await context.SaveChangesAsync(cancellationToken);
    }
}