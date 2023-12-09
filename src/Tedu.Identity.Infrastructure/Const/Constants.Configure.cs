using System.Net.Security;

namespace Tedu.Identity.Infrastructure.Const;

public static partial class SystemConstants
{
    public static class ConfigureOptions
    {
        public const string AssemblyName = "Tedu.Identity.IDP";
        public const string CorsPolicy = "CorsPolicy";
        public const string Roles = "roles";
        public const string UserName = "username";
        public const string FirstName = "first_name";
        public const string LastName = "last_name";
        public const string UserId = "userId";
        public const string Read = "tedu_microservices_api.read";
        public const string ReadDisplayName = "Tedu Microserives API Read Scope";
        public const string Write = "tedu_microservices_api.write";
        public const string WriteDisplayName = "Tedu Microserives API Write Scope";
        public const string Name = "tedu_microservices_api";
        public const string DisplayName = "Tedu Microservices API";
        public const string Administrator = nameof(Administrator);
        public const string Customer = nameof(Customer);
        public const string Version1 = "v1";
        public const string Title = "Tedu Identity Server Api";
        public const string SwaggerName = "Tedu Identity Server";
        public const string Email = "khoiphanngoc89@gmail.com";
        public const string Bearer = nameof(Bearer);
        public const string Url = "/swagger/v1/swagger.json";
        public const string EndpointName = "Tedu Identity API";
    }

    public static class PostmanClients
    {
        public const string ClientName = "Tedu Microservices Postman Client";
        public const string ClientId = "tedu_microservices_postman";
        public const string ClientSecret = "SuperStrongSecret";
    }

    public static class TeduClients
    {
        public const string ClientName = "Tedu Microservices Swagger Client";
        public const string ClientId = "tedu_microservices_swagger";
        public const int TokenLifeTime = 60 * 60 * 2;
    }
}
