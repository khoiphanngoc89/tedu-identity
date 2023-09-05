namespace Tedu.Identity.Common.Const;

public static partial class SystemConstants
{
    public const string Roles = "roles";
    public const string AssemblyName = "Tedu.Identity.IDP";

    public static class Claims
    {
        public const string Roles = "roles";
        public const string UserName = "username";
        public const string FirstName = "first_name";
        public const string LastName = "last_name";
        public const string UserId = "userId";
    }

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

    public static class TeduClients
    {
        public static class Swagger
        {
            public const string ClientName = "Tedu Microservices Swagger Client";
            public const string ClientId = "tedu_microservices_swagger";
            public const int TokenLifeTime = 60 * 60 * 2;
        }

        public static class Postman
        {
            public const string ClientName = "Tedu Microservices Postman Client";
            public const string ClientId = "tedu_microservices_postman";
        }
    }
}
