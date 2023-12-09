namespace Tedu.Identity.Infrastructure.Const;

public static partial class SystemConstants
{
    public static class Database
    {
        public static class Schemes
        {
            public const string IdentityScheme = "Identity";
        }

        public static class TableNames
        {
            public const string Role = nameof(Role);
            public const string User = nameof(User);
            public const string RoleClaim = nameof(RoleClaim);
            public const string UserRole = nameof(UserRole);
            public const string UserClaim = nameof(UserClaim);
            public const string UserLogin = nameof(UserLogin);
            public const string UserLogout = nameof(UserLogout);
            public const string UserToken = nameof(UserToken);
            public const string Permissions = nameof(Permissions);
        }

        public static class DataTypes
        {
            public const string Varchar50 = "varchar(50)";
            public const string Varchar255 = "varchar(255)";
            public const string Varchar20 = "varchar(20)";
            public const string Varchar150 = "varchar(150)";
            public const string Nvarchar150 = "nvarchar(150)";
        }

        public static class Seeds
        {
            public const string FirstName = "Alice";
            public const string LastName = "Smith";
            public const string Address = "Wollongong";
            public const string Password = "alice123";
            public const string Email = "alice.smith@example.com";
            public const string Role = "Administrator";
        }
    }
}
