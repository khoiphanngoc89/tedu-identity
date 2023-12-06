using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Tedu.Identity.Infrastructure.Enities;
using Tedu.Identity.Infrastructure.Repositories;

namespace Tedu.Identity.Infrastructure;

public interface IRepositoryManager
{
    UserManager<User> UserManager { get; }
    RoleManager<IdentityRole> RoleManager { get; }
    IPermissionRepository Permission { get; }
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task EndTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
