using Duende.IdentityServer.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tedu.Identity.Infrastructure.Const;
using Tedu.Identity.Infrastructure.Enities;
using Tedu.Identity.Infrastructure.Persistence;

namespace Tedu.Identity.IDP.Persistence;

public class SeedUser
{
    public static void EnsureSeedUser(string connectionString)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TeduIdentityContext>(opt => opt.UseSqlServer(connectionString));

        services.AddIdentity<User, IdentityRole>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequiredLength = 6;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<TeduIdentityContext>()
        .AddDefaultTokenProviders();

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        CreateUser(scope, GenerateDefaultUser());
    }

    private static UserInfo GenerateDefaultUser()
    => new()
    {
        FirstName = SystemConstants.Database.Seeds.FirstName,
        LastName = SystemConstants.Database.Seeds.LastName,
        Address = SystemConstants.Database.Seeds.Address,
        Id = Guid.NewGuid().ToString(),
        Password = SystemConstants.Database.Seeds.Password,
        Email = SystemConstants.Database.Seeds.Email,
        Role = SystemConstants.Roles.Administrator
    };

    private static void CreateUser(IServiceScope scope, UserInfo userInfo)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = userManager.FindByNameAsync(userInfo.Email).Result;
        IdentityResult result;

        if (user is not null)
        {
            return;
        }

        user = new()
        {
            UserName = userInfo.Email,
            Email = userInfo.Email,
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            Address = userInfo.Address,
            EmailConfirmed = true,
            Id = Guid.NewGuid().ToString(),
        };

        result = userManager.CreateAsync(user, userInfo.Password).Result;
        Preconditions(result);
        var addRole = userManager.AddToRoleAsync(user, userInfo.Role).Result;
        Preconditions(addRole);

        result = userManager.AddClaimsAsync(user, new Claim[]
        {
                new (SystemConstants.Claims.UserName, user.UserName!),
                new (SystemConstants.Claims.FirstName, user.FirstName),
                new (SystemConstants.Claims.LastName, user.LastName),
                new (SystemConstants.Claims.Roles, userInfo.Role),
                new (JwtClaimTypes.Address, user.Address),
                new (JwtClaimTypes.Email, user.Email!),
                new (ClaimTypes.NameIdentifier, user.Id)
        }).Result;

        Preconditions(result);


    }

    private static void Preconditions(IdentityResult result)
    {
        if (result.Succeeded)
        {
            return;
        }

        throw new InvalidDataException(result.Errors.First().Description);
    }

    internal class UserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
