using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;
using Tedu.Identity.Infrastructure.Exceptions;
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

    public IQueryable<TEntity> FindAll(bool trackChanges = false)
        => !trackChanges ? dbContext.Set<TEntity>().AsNoTracking() :
        dbContext.Set<TEntity>();

    public IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
        => !trackChanges
        ? dbContext.Set<TEntity>().Where(expression).AsNoTracking()
        : dbContext.Set<TEntity>().Where(expression);

    public IQueryable<TEntity> FindByCondition(
        Expression<Func<TEntity, bool>> expression,
        bool trackChanges = false,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => FindByCondition(x => x.Id!.Equals(id)).FirstOrDefaultAsync(cancellationToken);

    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        => FindByCondition(x => x.Id!.Equals(id), trackChanges: false, includeProperties).FirstOrDefaultAsync(cancellationToken);

    #endregion Query

    #region Action

    public async Task<TKey> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
        await SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
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

    public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().RemoveRange(entities);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var item = await FindByCondition(x => x.Id!.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        if (item is null)
        {
            return;
        }

        dbContext.Set<TEntity>().Remove(item);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
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

    public async Task<IReadOnlyList<TModel>> QueryAsync<TModel>(string query,
                                                                object? parameters,
                                                                CommandType commandType = CommandType.StoredProcedure,
                                                                IDbTransaction? transaction = default,
                                                                int? timeout = 30,
                                                                CancellationToken cancellationToken = default)
        where TModel : EntityBase<TKey>
    {
        var data = await this.dbContext.Connection.QueryAsync<TModel>(new (query, parameters, transaction, timeout, commandType, cancellationToken: cancellationToken));
        return data.AsList();
    }

    public async Task <TModel> QueryFirstOrDefaultAsync<TModel>(string query,
                                                                object? parameters,
                                                                CommandType commandType = CommandType.StoredProcedure,
                                                                IDbTransaction? transaction = default,
                                                                int? timeout = 30,
                                                                CancellationToken cancellationToken = default)
        where TModel : EntityBase<TKey>
    {
        var data = await this.dbContext.Connection.QueryFirstOrDefaultAsync<TModel>(new (query, parameters, transaction, timeout, commandType, cancellationToken: cancellationToken));
        if (data is null)
        {
            throw new EntityNotFoundException();
        }
        return data!;
    }

    public async Task<TModel> QuerySingleOrDefaultAsync<TModel>(string query,
                                                                object? parameters,
                                                                CommandType commandType = CommandType.StoredProcedure,
                                                                IDbTransaction? transaction = default,
                                                                int? timeout = 30,
                                                                CancellationToken cancellationToken = default)
        where TModel : EntityBase<TKey>
        => await this.dbContext.Connection.QuerySingleAsync<TModel>(new(query, parameters, transaction, timeout, commandType, cancellationToken: cancellationToken));

    public Task<int> ExecuteAsync(string query,
                                  object? parameters,
                                  CommandType commandType = CommandType.StoredProcedure,
                                  IDbTransaction? transaction = default,
                                  int? timeout = 30,
                                  CancellationToken cancellationToken = default)
        => this.dbContext.Connection.ExecuteAsync(new(query, parameters, transaction, timeout, commandType, cancellationToken: cancellationToken));

    #endregion

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => dbContext.Database.BeginTransactionAsync(cancellationToken);

    public async Task EndTransactionAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        await dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        => dbContext.Database.RollbackTransactionAsync(cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => unitOfWork.CommitAsync(cancellationToken);
}