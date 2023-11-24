namespace Tedu.Identity.Infrastructure.Domains;

public interface IEntityBase<T>
{
    T Id { get; set; }
}
