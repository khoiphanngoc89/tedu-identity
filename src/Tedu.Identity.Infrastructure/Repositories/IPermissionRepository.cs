using Tedu.Identity.Infrastructure.Enities;
using Tedu.Identity.Infrastructure.Domains;
using Tedu.Identity.Infrastructure.ViewModels;

namespace Tedu.Identity.Infrastructure.Repositories;

public interface IPermissionRepository : IRepositoryBase<long, Permission>
{
    Task<IReadOnlyList<PermissionViewModel>> GetPermissionsByRoleAsync(string roleId, CancellationToken cancellationToken = default);
    Task UpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingViewModel> permissions, bool trackChanges, CancellationToken cancellationToken = default);
}
