using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Tedu.Identity.Infrastructure.Enities;
using Tedu.Identity.Infrastructure.Domains;
using Tedu.Identity.IDP.Persistence;
using Tedu.Identity.Infrastructure.Repositories;

namespace Tedu.Identity.Infrastructure;

public class RepositoryManager : IRepositoryManager
{
    private readonly IUnitOfWork unitOfWork;
    private readonly TeduIdentityContext context;
    private readonly Lazy<IPermissionRepository> permissionRepository;
    public UserManager<User> UserManager { get; }
    public RoleManager<IdentityRole> RoleManager { get; }
    public IPermissionRepository PermissionRepository => this.permissionRepository.Value;

    public RepositoryManager(
        TeduIdentityContext dbContext,
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.unitOfWork = unitOfWork;
        this.context = dbContext;
        this.UserManager = userManager;
        this.RoleManager = roleManager;

        this.permissionRepository = new Lazy<IPermissionRepository>(() => new PermissionRepository(dbContext, unitOfWork));
    }

    private Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    => this.context.Database.BeginTransactionAsync(cancellationToken);

    private Task EndTransactionAsync(CancellationToken cancellationToken)
    => this.context.Database.CommitTransactionAsync(cancellationToken);

    private Task RollbackTransactionAsync(CancellationToken cancellationToken)
    => this.context.Database.RollbackTransactionAsync(cancellationToken);

    private Task<int> SaveAsync(CancellationToken cancellationToken)
    => this.unitOfWork.CommitAsync(cancellationToken);

    #region Implement of interface
    Task<IDbContextTransaction> IRepositoryManager.BeginTransactionAsync(CancellationToken cancellationToken)
    => this.BeginTransactionAsync(cancellationToken);
    Task IRepositoryManager.EndTransactionAsync(CancellationToken cancellationToken)
    => this.EndTransactionAsync(cancellationToken);
    Task IRepositoryManager.RollbackTransactionAsync(CancellationToken cancellationToken)
    => this.RollbackTransactionAsync(cancellationToken);
    Task<int> IRepositoryManager.SaveAsync(CancellationToken cancellationToken)
    => this.SaveAsync(cancellationToken);
    #endregion
}
