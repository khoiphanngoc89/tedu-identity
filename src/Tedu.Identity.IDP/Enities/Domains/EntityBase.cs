namespace Tedu.Identity.IDP.Enities.Domains;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public required TKey Id { get; set; }
}
