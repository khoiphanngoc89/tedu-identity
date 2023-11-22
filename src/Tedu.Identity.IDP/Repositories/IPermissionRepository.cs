using Tedu.Identity.IDP.Enities;
using Tedu.Identity.IDP.Enities.Domains;
using Tedu.Identity.IDP.ViewModels;

namespace Tedu.Identity.IDP.Repositories;

public interface IPermissionRepository : IRepositoryBase<long, Permission>
{
    Task<IReadOnlyList<PermissionViewModel>> GetPermissionsByRoleAsync(string roleId, CancellationToken cancellationToken = default);
    Task UpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingViewModel> permissions, bool trackChanges, CancellationToken cancellationToken = default);
}
