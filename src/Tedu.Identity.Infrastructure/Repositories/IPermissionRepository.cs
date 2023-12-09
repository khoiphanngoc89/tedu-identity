using Tedu.Identity.Infrastructure.Entities;
using Tedu.Identity.Infrastructure.Domains;
using Tedu.Identity.Infrastructure.ViewModels;

namespace Tedu.Identity.Infrastructure.Repositories;

public interface IPermissionRepository : IRepositoryBase<long, Permission>
{
    Task<IReadOnlyList<PermissionResponse>> GetAllByRoleAsync(string roleId, CancellationToken cancellationToken = default);
    Task<PermissionResponse?> CreatePermissionAsync(string roleId, PermissionAddingRequest model, CancellationToken cancellationToken = default);
    Task DeletePermissionAsync(string roleId, string function, string command, CancellationToken cancellationToken = default);
    Task UpdatePermissionsByRoleIdAsync(string roleId, IEnumerable<PermissionAddingRequest> permissions, CancellationToken cancellationToken = default);
}
