using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;

namespace Tedu.Identity.Infrastructure.Domains;

public interface IRepositoryBase<TKey, TEntity>
    where TEntity : EntityBase<TKey>
{
    #region Query
    IQueryable<TEntity> FindAll(bool trackChanges = false);
    IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false);
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression,
                                        bool trackChanges = false,
                                        params Expression<Func<TEntity, object>>[] includeProperties);

    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);
    #endregion

    #region Action
    Task<TKey> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    Task DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
    #endregion

    #region Dapper

    Task<IReadOnlyList<TModel>> QueryAsync<TModel>(string query,
                                                   object? parameters,
                                                   CommandType commandType,
                                                   IDbTransaction? transaction,
                                                   int? timeout,
                                                   CancellationToken cancellationToken = default)
        where TModel: EntityBase<TKey>;

    Task<TModel> QueryFirstOrDefaultAsync<TModel>(string query,
                                                  object? parameters,
                                                  CommandType commandType,
                                                  IDbTransaction? transaction,
                                                  int? timeout,
                                                  CancellationToken cancellationToken = default)
        where TModel : EntityBase<TKey>;

    Task<TModel> QuerySingleOrDefaultAsync<TModel>(string query,
                                                   object? parameters,
                                                   CommandType commandType,
                                                   IDbTransaction? transaction,
                                                   int? timeout,
                                                   CancellationToken cancellationToken = default)
        where TModel : EntityBase<TKey>;

    Task<int> ExcuteAsync(string query,
                           object? parameters,
                           CommandType commandType,
                           IDbTransaction? transaction,
                           int? timeout,
                           CancellationToken cancellationToken = default);

    #endregion
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task EndTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
