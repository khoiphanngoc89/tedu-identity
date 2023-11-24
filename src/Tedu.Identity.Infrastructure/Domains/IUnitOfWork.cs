namespace Tedu.Identity.Infrastructure.Domains;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
