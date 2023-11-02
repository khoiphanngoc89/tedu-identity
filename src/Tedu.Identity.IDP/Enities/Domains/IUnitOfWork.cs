namespace Tedu.Identity.IDP.Enities.Domains;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
