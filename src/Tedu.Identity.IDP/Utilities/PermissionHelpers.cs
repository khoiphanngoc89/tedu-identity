namespace Tedu.Identity.IDP.Utilities;

public static class PermissionHelpers
{
    public static string GetPermission(string function, string command)
        => string.Join(".", function, command);
}
