namespace Tedu.Contracts.Domains;

public interface IEntityBase<TKey>
{
    TKey Id { get; set; }
}
