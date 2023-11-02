namespace Tedu.Identity.IDP.Enities;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}
