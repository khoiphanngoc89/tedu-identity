namespace Tedu.Identity.Common.Const;

public static partial class SystemConstants
{
    public static class Database
    {
        public const string IdentityScheme = "Identity";
        public static class TableName
        {
            public const string Role = nameof(Role);
            public const string User = nameof(User);
            public const string RoleClaim = nameof(RoleClaim);
            public const string UserRole = nameof(UserRole);
            public const string UserClaim = nameof(UserClaim);
            public const string UserLogin = nameof(UserLogin);
        }

        public static class Seeds
        {
            public const string FirstName = "Alice";
            public const string LastName = "Smith";
            public const string Address = "Wollongong";
            public const string Password = "alice123";
            public const string Email = "alice.smith@example.com";
            public const string Role = "Adminstrator";
        }    
    }
}
