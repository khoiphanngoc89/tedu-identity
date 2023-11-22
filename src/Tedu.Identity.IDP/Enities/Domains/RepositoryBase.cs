using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Tedu.Identity.IDP.Persistence;

namespace Tedu.Identity.IDP.Enities.Domains;

public class RepositoryBase<TKey, TEntity> : IRepositoryBase<TKey, TEntity>
    where TEntity : EntityBase<TKey>
{
    private readonly TeduIdentityContext dbContext;
    private readonly IUnitOfWork unitOfWork;

    public RepositoryBase(TeduIdentityContext dbContext, IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    #region Query

    private IQueryable<TEntity> FindAll(bool trackChanges)
    => !trackChanges ? this.dbContext.Set<TEntity>().AsNoTracking() :
        this.dbContext.Set<TEntity>();

    private IQueryable<TEntity> FindAll(bool trackChanges, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = this.FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    private IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
     => !trackChanges
        ? this.dbContext.Set<TEntity>().Where(expression).AsNoTracking()
        : this.dbContext.Set<TEntity>().Where(expression);

    private IQueryable<TEntity> FindByCondition(
        Expression<Func<TEntity, bool>> expression,
        bool trackChanges,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = this.FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    private Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    => this.FindByCondition(x => x.Id!.Equals(id)).FirstOrDefaultAsync(cancellationToken);

    private Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
    => this.FindByCondition(x => x.Id!.Equals(id), trackChanges: false, includeProperties).FirstOrDefaultAsync(cancellationToken);

    #endregion Query

    #region Action

    private async Task<TKey> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await this.dbContext.Set<TEntity>().AddAsync(entity);
        await this.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    private async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (this.dbContext.Entry(entity).State == EntityState.Unchanged)
        {
            return;
        }

        TEntity? exist = await this.dbContext.Set<TEntity>().FindAsync(entity.Id);
        if (exist is null)
        {
            return;
        }
        this.dbContext.Entry(exist).CurrentValues.SetValues(entity);
        await this.SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await this.dbContext.Set<TEntity>().AddRangeAsync(entities);
        await this.SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        this.dbContext.Set<TEntity>().Remove(entity);
        await this.SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        this.dbContext.Set<TEntity>().RemoveRange(entities);
        await this.SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        var item = await this.FindByCondition(x => x.Id!.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        if (item is null)
        {
            return;
        }

        this.dbContext.Set<TEntity>().Remove(item);
        await this.SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken)
    {
        var items = await this.FindByCondition(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        if (items is null)
        {
            return;
        }

        this.dbContext.Set<TEntity>().RemoveRange(items);
        await this.SaveChangesAsync(cancellationToken);
    }

    #endregion Action

    private Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        => this.dbContext.Database.BeginTransactionAsync(cancellationToken);

    private async Task EndTransactionAsync(CancellationToken cancellationToken)
    {
        await this.SaveChangesAsync(cancellationToken);
        await this.dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    private Task RollbackTransactionAsync(CancellationToken cancellationToken)
    => this.dbContext.Database.RollbackTransactionAsync(cancellationToken);

    private Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    => this.unitOfWork.CommitAsync(cancellationToken);

    #region Implement of interface

    IQueryable<TEntity> IRepositoryBase<TKey, TEntity>.FindAll(bool trackChanges)
        => this.FindAll(trackChanges);

    IQueryable<TEntity> IRepositoryBase<TKey, TEntity>.FindAll(bool trackChanges, params Expression<Func<TEntity, object>>[] includeProperties)
        => this.FindAll(trackChanges, includeProperties);

    IQueryable<TEntity> IRepositoryBase<TKey, TEntity>.FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges)
        => this.FindByCondition(expression, trackChanges);

    IQueryable<TEntity> IRepositoryBase<TKey, TEntity>.FindByCondition(
        Expression<Func<TEntity, bool>> expression,
        bool trackChanges,
        params Expression<Func<TEntity, object>>[] includeProperties)
        => this.FindByCondition(expression, trackChanges, includeProperties);

    Task<TEntity?> IRepositoryBase<TKey, TEntity>.GetByIdAsync(TKey id, CancellationToken cancellationToken)
        => this.GetByIdAsync(id, cancellationToken);

    Task<TEntity?> IRepositoryBase<TKey, TEntity>.GetByIdAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        => this.GetByIdAsync(id, cancellationToken, includeProperties);

    Task<TKey> IRepositoryBase<TKey, TEntity>.CreateAsync(TEntity entity, CancellationToken cancellationToken)
        => this.CreateAsync(entity, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        => this.UpdateAsync(entity, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        => this.UpdateAsync(entities, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        => this.DeleteAsync(entity, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        => this.DeleteAsync(entities, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.DeleteAsync(TKey id, CancellationToken cancellationToken)
        => this.DeleteAsync(id, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken)
        => this.DeleteAsync(ids, cancellationToken);

    Task<IDbContextTransaction> IRepositoryBase<TKey, TEntity>.BeginTransactionAsync(CancellationToken cancellationToken)
        => this.BeginTransactionAsync(cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.EndTransactionAsync(CancellationToken cancellationToken)
        => this.EndTransactionAsync(cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.RollbackTransactionAsync(CancellationToken cancellationToken)
        => this.RollbackTransactionAsync(cancellationToken);

    Task<int> IRepositoryBase<TKey, TEntity>.SaveChangesAsync(CancellationToken cancellationToken)
        => this.SaveChangesAsync(cancellationToken);

    #endregion Implement of interface
}