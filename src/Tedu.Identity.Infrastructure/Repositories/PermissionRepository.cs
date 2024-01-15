using Tedu.Identity.Infrastructure.Entities;
using Tedu.Identity.Infrastructure.Domains;
using Tedu.Identity.Infrastructure.Persistence;
using Tedu.Identity.Infrastructure.Models;
using Tedu.Identity.Infrastructure.Repositories;
using Dapper;
using System.Data;
using Tedu.Identity.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Tedu.Identity.Infrastructure.Models.Mappings;

namespace Tedu.Identity.Infrastructure;

public sealed class PermissionRepository : RepositoryBase<long, Permission>, IPermissionRepository
{
    private readonly UserManager<User> userManager;
    public PermissionRepository(TeduIdentityContext dbContext, IUnitOfWork unitOfWork, UserManager<User> userManager)
        : base(dbContext, unitOfWork)
    {
        this.userManager = userManager;
    }

    private Task<IReadOnlyList<PermissionResponse>> GetPermissionsByRoleAsync(string roleId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(roleId));
        return GetPermissionsByRoleInternalAsync(roleId, cancellationToken);
    }

    private async Task<IReadOnlyList<PermissionResponse>> GetPermissionsByRoleInternalAsync(string roleId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleId", roleId);
        var entity = await this.QueryAsync<PermissionResponse>("GetPermissionByRoleId", parameters, cancellationToken: cancellationToken);
        return entity;
    }

    private Task UpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingRequest> permissions, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleId);

        return this.HandleUpdatePermissionsByRoleIdAsync(roleId, permissions, cancellationToken);
    }

    private Task HandleUpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingRequest> permissions, CancellationToken cancellationToken)
    {
        var dt = new DataTable();
        dt.Columns.Add("RoleId", typeof(string));
        dt.Columns.Add("Function", typeof(string));
        dt.Columns.Add("Command", typeof(string));

        foreach (var permission in permissions)
        {
            dt.Rows.Add(roleId, permission.Function?.ToUpper(), permission.Command?.ToUpper());
        }

        var parameters = new DynamicParameters();
        parameters.Add("@roleId", roleId, DbType.String);
        parameters.Add("@permissions", dt.AsTableValuedParameter("dbo.Permissions"));
        return this.ExecuteAsync("UpdatePermissionByRole", parameters, cancellationToken: cancellationToken);
    }

    private Task<PermissionResponse?> CreatePermissionAsync(string roleId, PermissionAddingRequest model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleId);
        ArgumentNullException.ThrowIfNull(model);

        return this.HandleCreatePermissionAsync(roleId, model, cancellationToken);
    }

    private async Task<PermissionResponse?> HandleCreatePermissionAsync(string roleId, PermissionAddingRequest model, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleId", roleId, DbType.String);
        parameters.Add("@function", model.Function, DbType.String);
        parameters.Add("@command", model.Command, DbType.String);
        parameters.Add("@id", DbType.Int64, direction: ParameterDirection.Output);

        var result = await this.ExecuteAsync("CreatePermission", parameters, cancellationToken: cancellationToken);
        if (result <= 0)
        {
            return default;
        }

        var id = parameters.Get<dynamic>("@id");
        return new()
        {
            Id = DynamicHelpers.Convert<long>(id),
            Command = model.Command,
            Function = model.Function,
            RoleId = roleId
        };
    }

    private Task DeletePermissionAsync(string roleId, string function, string command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleId);
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(command);
        return this.HandleDeletePermissionAsync(roleId, function, command, cancellationToken);
    }

    private Task HandleDeletePermissionAsync(string roleId, string function, string command, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleId", roleId, DbType.String);
        parameters.Add("@function", function, DbType.String);
        parameters.Add("@command", command, DbType.String);
        return this.ExecuteAsync("DeletePermission", parameters, cancellationToken: cancellationToken);
    }

    private Task<IEnumerable<PermissionUserResponse>> GetPermissionByUserAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return this.HandleGetPermissionByUserAsync(user, cancellationToken);
    }

    private async Task<IEnumerable<PermissionUserResponse>> HandleGetPermissionByUserAsync(User user, CancellationToken cancellationToken)
    {
        var userRoles = await this.userManager.GetRolesAsync(user);
        var query = this.FindAll()
                          .Where(x => userRoles.Contains(x.RoleId))
                          .Select(x => new Permission(x.Id, x.Function!, x.Command!, x.RoleId));

        return query.ToList()!.Select(x => x.ToPermissionUserResponse());
    }

    #region Implemetation of IPermissionRepository

    Task<IReadOnlyList<PermissionResponse>> IPermissionRepository.GetAllByRoleAsync(string roleId, CancellationToken cancellationToken)
        => this.GetPermissionsByRoleAsync(roleId, cancellationToken);

    Task IPermissionRepository.UpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingRequest> permissions, CancellationToken cancellationToken)
        => this.UpdatePermissionsByRoleIdAsync(roleId, permissions, cancellationToken);

    Task<PermissionResponse?> IPermissionRepository.CreatePermissionAsync(string roleId, PermissionAddingRequest model, CancellationToken cancellationToken)
        => this.CreatePermissionAsync(roleId, model, cancellationToken);

    Task IPermissionRepository.DeletePermissionAsync(string roleId, string function, string command, CancellationToken cancellationToken)
        => this.DeletePermissionAsync(roleId, function, command, cancellationToken);

    Task<IEnumerable<PermissionUserResponse>> IPermissionRepository.GetPermissionByUserAsync(User user, CancellationToken cancellationToken = default)
        => this.GetPermissionByUserAsync(user, cancellationToken);

    #endregion
}
