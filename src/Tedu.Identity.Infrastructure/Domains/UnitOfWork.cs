using Tedu.Identity.IDP.Persistence;

namespace Tedu.Identity.Infrastructure.Domains;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TeduIdentityContext _context;
    public UnitOfWork(TeduIdentityContext context)
    {
        _context = context;
    }
    public Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
