using Tedu.Identity.IDP.Enities;
using Tedu.Identity.IDP.Enities.Domains;
using Tedu.Identity.IDP.Persistence;
using Tedu.Identity.IDP.ViewModels;

namespace Tedu.Identity.IDP.Repositories;

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

    private Task<IReadOnlyList<PermissionViewModel>> GetPermissionsByRoleInternalAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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

    #region Implemetation of IPermissionRepository

    Task<IReadOnlyList<PermissionViewModel>> IPermissionRepository.GetPermissionsByRoleAsync(string roleId, CancellationToken cancellationToken)
        => this.GetPermissionsByRoleAsync(roleId, cancellationToken);

    Task IPermissionRepository.UpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingViewModel> permissions, bool trackChanges, CancellationToken cancellationToken)
        => this.UpdatePermissionsByRoleIdAsync(roleId, permissions, trackChanges, cancellationToken);

    #endregion
}
