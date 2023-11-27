using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;
using Tedu.Identity.Infrastructure.Persistence;

namespace Tedu.Identity.Infrastructure.Domains;

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
        => !trackChanges ? dbContext.Set<TEntity>().AsNoTracking() :
        dbContext.Set<TEntity>();

    private IQueryable<TEntity> FindAll(bool trackChanges, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    private IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
        => !trackChanges
        ? dbContext.Set<TEntity>().Where(expression).AsNoTracking()
        : dbContext.Set<TEntity>().Where(expression);

    private IQueryable<TEntity> FindByCondition(
        Expression<Func<TEntity, bool>> expression,
        bool trackChanges,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    private Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
        => FindByCondition(x => x.Id!.Equals(id)).FirstOrDefaultAsync(cancellationToken);

    private Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        => FindByCondition(x => x.Id!.Equals(id), trackChanges: false, includeProperties).FirstOrDefaultAsync(cancellationToken);

    #endregion Query

    #region Action

    private async Task<TKey> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
        await SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    private async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (dbContext.Entry(entity).State == EntityState.Unchanged)
        {
            return;
        }

        TEntity? exist = await this.dbContext.Set<TEntity>().FindAsync(entity.Id);
        if (exist is null)
        {
            return;
        }
        dbContext.Entry(exist).CurrentValues.SetValues(entity);
        await SaveChangesAsync(cancellationToken);
    }

    private async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities);
        await SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        dbContext.Set<TEntity>().RemoveRange(entities);
        await SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync(TKey id, CancellationToken cancellationToken)
    {
        var item = await FindByCondition(x => x.Id!.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        if (item is null)
        {
            return;
        }

        dbContext.Set<TEntity>().Remove(item);
        await SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken)
    {
        var items = await FindByCondition(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
        if (items is null)
        {
            return;
        }

        dbContext.Set<TEntity>().RemoveRange(items);
        await SaveChangesAsync(cancellationToken);
    }

    #endregion Action

    #region Dapper

    private async Task<IReadOnlyList<TModel>> QueryAsync<TModel>(string query, object? parameters, CommandType commandType, IDbTransaction? transaction, int? timeout, CancellationToken cancellationToken) where TModel : EntityBase<TKey>
    {
        var data = await this.dbContext.Connection.QueryAsync<TModel>(new (query, parameters, transaction, timeout, commandType, cancellationToken: cancellationToken));
        return data.ToList();
    }

    private async Task <TModel> QueryFirstOrDefaultAsync<TModel>(string query, object? parameters, CommandType commandType, IDbTransaction? transaction, int? timeout, CancellationToken cancellationToken) where TModel : EntityBase<TKey>
    {
        var data = await this.dbContext.Connection.QueryFirstOrDefaultAsync<TModel>(new (query, parameters, transaction, timeout, commandType, cancellationToken: cancellationToken));
        return data!;
    }

    private async Task<TModel> QuerySingleOrDefault<TModel>(string query, object? parameters, CommandType commandType, IDbTransaction? transaction, int? timeout, CancellationToken cancellationToken)
    {
        var data = await this.dbContext.Connection.QuerySingleAsync<TModel>(new(query, parameters, transaction, timeout, commandType, cancellationToken: cancellationToken));
        return data!;
    }

    private Task<int> ExecuteAsync(string query, object? parameters, CommandType commandType, IDbTransaction? transaction, int? timeout, CancellationToken cancellationToken)
        => this.dbContext.Connection.ExecuteAsync(new(query, parameters, transaction, timeout, commandType, cancellationToken: cancellationToken));

    #endregion

    private Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        => dbContext.Database.BeginTransactionAsync(cancellationToken);

    private async Task EndTransactionAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
        await dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    private Task RollbackTransactionAsync(CancellationToken cancellationToken)
    => dbContext.Database.RollbackTransactionAsync(cancellationToken);

    private Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    => unitOfWork.CommitAsync(cancellationToken);

    #region Implement of interface

    IQueryable<TEntity> IRepositoryBase<TKey, TEntity>.FindAll(bool trackChanges)
        => FindAll(trackChanges);

    IQueryable<TEntity> IRepositoryBase<TKey, TEntity>.FindAll(bool trackChanges, params Expression<Func<TEntity, object>>[] includeProperties)
        => FindAll(trackChanges, includeProperties);

    IQueryable<TEntity> IRepositoryBase<TKey, TEntity>.FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges)
        => FindByCondition(expression, trackChanges);

    IQueryable<TEntity> IRepositoryBase<TKey, TEntity>.FindByCondition(
        Expression<Func<TEntity, bool>> expression,
        bool trackChanges,
        params Expression<Func<TEntity, object>>[] includeProperties)
        => FindByCondition(expression, trackChanges, includeProperties);

    Task<TEntity?> IRepositoryBase<TKey, TEntity>.GetByIdAsync(TKey id, CancellationToken cancellationToken)
        => GetByIdAsync(id, cancellationToken);

    Task<TEntity?> IRepositoryBase<TKey, TEntity>.GetByIdAsync(TKey id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        => GetByIdAsync(id, cancellationToken, includeProperties);

    Task<TKey> IRepositoryBase<TKey, TEntity>.CreateAsync(TEntity entity, CancellationToken cancellationToken)
        => CreateAsync(entity, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        => UpdateAsync(entity, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        => UpdateAsync(entities, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        => DeleteAsync(entity, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        => DeleteAsync(entities, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.DeleteAsync(TKey id, CancellationToken cancellationToken)
        => DeleteAsync(id, cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken)
        => DeleteAsync(ids, cancellationToken);

    Task<IDbContextTransaction> IRepositoryBase<TKey, TEntity>.BeginTransactionAsync(CancellationToken cancellationToken)
        => BeginTransactionAsync(cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.EndTransactionAsync(CancellationToken cancellationToken)
        => EndTransactionAsync(cancellationToken);

    Task IRepositoryBase<TKey, TEntity>.RollbackTransactionAsync(CancellationToken cancellationToken)
        => RollbackTransactionAsync(cancellationToken);

    Task<int> IRepositoryBase<TKey, TEntity>.SaveChangesAsync(CancellationToken cancellationToken)
        => SaveChangesAsync(cancellationToken);

    Task<IReadOnlyList<TModel>> IRepositoryBase<TKey, TEntity>.QueryAsync<TModel>(string query, object? parameters, CommandType commandType, IDbTransaction? transaction, int? timeout, CancellationToken cancellationToken)
        => this.QueryAsync<TModel>(query, parameters, commandType, transaction, timeout, cancellationToken);

    Task<TModel> IRepositoryBase<TKey, TEntity>.QueryFirstOrDefaultAsync<TModel>(string query, object? parameters, CommandType commandType, IDbTransaction? transaction, int? timeout, CancellationToken cancellationToken)
        => this.QueryFirstOrDefaultAsync<TModel>(query, parameters, commandType, transaction, timeout, cancellationToken);
    Task<TModel> IRepositoryBase<TKey, TEntity>.QuerySingleOrDefaultAsync<TModel>(string query, object? parameters, CommandType commandType, IDbTransaction? transaction, int? timeout, CancellationToken cancellationToken)
        => this.QuerySingleOrDefault<TModel>(query, parameters, commandType, transaction, timeout, cancellationToken);

    Task<int> IRepositoryBase<TKey, TEntity>.ExcuteAsync(string query, object? parameters, CommandType commandType, IDbTransaction? transaction, int? timeout, CancellationToken cancellationToken)
        => this.ExecuteAsync(query, parameters, commandType, transaction, timeout, cancellationToken);

    #endregion Implement of interface
}