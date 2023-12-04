using Tedu.Identity.Infrastructure.Enities;
using Tedu.Identity.Infrastructure.Domains;
using Tedu.Identity.Infrastructure.Persistence;
using Tedu.Identity.Infrastructure.ViewModels;
using Tedu.Identity.Infrastructure.Repositories;
using Dapper;
using System.Data;
using Tedu.Identity.Infrastructure.Helpers;

namespace Tedu.Identity.Infrastructure;

public sealed class PermissionRepository : RepositoryBase<long, Permission>, IPermissionRepository
{
    public PermissionRepository(TeduIdentityContext dbContext, IUnitOfWork unitOfWork)
        : base(dbContext, unitOfWork)
    {
    }

    private Task<IReadOnlyList<PermissionViewModel>> GetPermissionsByRoleAsync(string roleId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(roleId))
        {
            throw new ArgumentNullException(nameof(roleId));
        }

        return GetPermissionsByRoleInternalAsync(roleId, cancellationToken);
    }

    private async Task<IReadOnlyList<PermissionViewModel>> GetPermissionsByRoleInternalAsync(string roleId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleId", roleId);
        var entity = await QueryAsync<PermissionViewModel>("GetPermissionByRoleId", parameters, cancellationToken: cancellationToken);
        return entity;
    }

    private Task UpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingViewModel> permissions, bool trackChanges, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(roleId))
        {
            throw new ArgumentNullException(nameof(roleId));
        }

        return this.UpdatePermissionsByRoleIdInternalAsync(roleId, permissions, trackChanges, cancellationToken);
    }

    private Task UpdatePermissionsByRoleIdInternalAsync(string roleId, IEnumerable<PermissionAddingViewModel> permissions, bool trackChanges, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private Task<PermissionViewModel?> CreatePermissionAsync(string roleId, PermissionAddingViewModel model, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(roleId))
        {
            throw new ArgumentNullException(nameof(roleId));
        }

        if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        return this.CreatePermissionInternalAsync(roleId, model, cancellationToken);
    }

    private async Task<PermissionViewModel?> CreatePermissionInternalAsync(string roleId, PermissionAddingViewModel model, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleId", roleId, DbType.String);
        parameters.Add("@function", model.Function, DbType.String);
        parameters.Add("@command", model.Command, DbType.String);
        parameters.Add("@insertedId", DbType.Int64, direction: ParameterDirection.Output);

        var result = await this.ExecuteAsync("CreatePermission", parameters, cancellationToken: cancellationToken);
        if (result <= 0)
        {
            return default;
        }

        var id = parameters.Get<dynamic>("@insertedId");
        return new()
        {
            Id = DynamicHelpers.Convert<long>(id),
            Command = model.Command,
            Function = model.Function,
            RoleId = roleId
        };
    }

    #region Implemetation of IPermissionRepository

    Task<IReadOnlyList<PermissionViewModel>> IPermissionRepository.GetAllByRoleAsync(string roleId, CancellationToken cancellationToken)
        => this.GetPermissionsByRoleAsync(roleId, cancellationToken);

    Task IPermissionRepository.UpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingViewModel> permissions, bool trackChanges, CancellationToken cancellationToken)
        => this.UpdatePermissionsByRoleIdAsync(roleId, permissions, trackChanges, cancellationToken);

    Task<PermissionViewModel?> IPermissionRepository.CreatePermissionAsync(string roleId, PermissionAddingViewModel model, CancellationToken cancellationToken)
        => this.CreatePermissionAsync(roleId, model, cancellationToken);

    #endregion
}
