namespace Tedu.Identity.Infrastructure.Exceptions;

public sealed class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException()
        : base("Entity was not found")
    {
    }
}
