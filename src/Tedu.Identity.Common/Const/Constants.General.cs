namespace Tedu.Identity.Common.Const;

public static partial class Constants
{
    public const string Role = "role";

    public static class TeduScopes
    {
        public const string Read = "tedu_microservices_api.read";
        public const string ReadDisplayName = "Tedu Microserives API Read Scope";
        public const string Write = "tedu_microservices_api.write";
        public const string WriteDisplayName = "Tedu Microserives API Write Scope";
    }
    
    public static class TeduApiResources
    {
        public const string Name = "tedu_microservice_api";
        public const string DisplayName = "Tedu Microservices API";
    }    
}