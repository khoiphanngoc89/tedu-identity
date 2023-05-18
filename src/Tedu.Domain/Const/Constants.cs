namespace Tedu.Domain.Const;

public static partial class Constants
{
    public static class IdentityResources
    {
        public const string Role = "role";
    }

    public static class StandardScopes
    {
        public const string Read = "tedu_microservices_api.read";
        public const string Write = "tedu_microservices_api.write";
        public const string ReadDisplayName = "Tedu Microservices Api Read Scope";
        public const string WriteDisplayName = "Tedu Microservices Api Write Scope";
    }
}