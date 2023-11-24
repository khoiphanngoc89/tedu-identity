namespace Tedu.Identity.Infrastructure.Domains;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public required TKey Id { get; set; }
}
