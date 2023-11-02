namespace Tedu.Identity.IDP.Enities.Domains;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}
