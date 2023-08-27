using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tedu.Identity.Common.Const;
using Tedu.Identity.IDP.Enities;

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
        CreateUser(scope, new()
        {
            FirstName = "Alice",
            LastName = "Smith",
            Address = "Wollongong",
            Id = Guid.NewGuid().ToString(),
            Password = "alice123",
            Email = "alice.smith@example.com",
            Role = "Administrator"
        });
    }

    private static void CreateUser(IServiceScope scope, UserInfo userInfo)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = userManager.FindByNameAsync(userInfo.Email).Result;
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

        var result = userManager.CreateAsync(user).Result;
        CheckResult(result);

        var addRole = userManager.AddToRoleAsync(user, userInfo.Role).Result;

        result = userManager.AddClaimsAsync(user, new Claim[]
        {
            new(SystemConstants.Claim.UserName, user.UserName),
            new(SystemConstants.Claim.FirstName, user.FirstName),
            new(SystemConstants.Claim.LastName, user.LastName),
            new(SystemConstants.Claim.Roles, userInfo.Role),
            new(JwtClaimTypes.Address, user.Address),
            new(JwtClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        }).Result;

        CheckResult(result);

    }

    private static void CheckResult(IdentityResult result)
    {
        if(result.Succeeded)
        {
            return;
        }

        throw new Exception(result.Errors.First().Description);
    }    

    internal class UserInfo
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
