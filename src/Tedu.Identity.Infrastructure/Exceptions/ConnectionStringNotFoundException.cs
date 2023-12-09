namespace Tedu.Identity.Infrastructure.Exceptions;

public sealed class ConnectionStringNotFoundException : Exception
{
    public ConnectionStringNotFoundException()
        : base("Connectionstrings not found.")
    {

    }
}
