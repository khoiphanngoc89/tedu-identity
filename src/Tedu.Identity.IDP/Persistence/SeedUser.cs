using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tedu.Contracts.Entities;
using Tedu.Infrastructure.Persistence;

namespace Tedu.Identity.IDP.Persistence;

public static class SeedUser
{
    public static void EnsureSeedData(string connectionString)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TeduIdentityContext>(opt =>
            opt.UseSqlServer(connectionString));

        services.AddIdentity<User, IdentityRole>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequiredLength = 6;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<TeduIdentityContext>()
            .AddDefaultTokenProviders();

        using (var serviceProvider = services.BuildServiceProvider())
        {
            using (var scope = serviceProvider
                       .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                CreateUser(scope, "Alice", "Smith", "Alice Smith's Wollongong",
                    Guid.NewGuid().ToString(), "alice123",
                    "Administrator", "alicesmith@example.com");
            }
        }
    }

    private static void CreateUser(IServiceScope scope, string firstName, string lastName,
        string address, string id, string password, string role, string email)
    {
        var userManagement = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = userManagement.FindByNameAsync(email).Result;
        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                EmailConfirmed = true,
                Id = id,
            };
            var result = userManagement.CreateAsync(user, password).Result;
            CheckResult(result);

            var addToRoleResult = userManagement.AddToRoleAsync(user, role).Result;
            CheckResult(addToRoleResult);

            result = userManagement.AddClaimsAsync(user, new Claim[]
            {
                new(Contracts.Const.SystemConstants.Claims.UserName, user.UserName),
                new(Contracts.Const.SystemConstants.Claims.FirstName, user.FirstName),
                new(Contracts.Const.SystemConstants.Claims.LastName, user.LastName),
                new(Contracts.Const.SystemConstants.Claims.Roles, role),
                new(JwtClaimTypes.Address, user.Address),
                new(JwtClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id),
            }).Result;
            CheckResult(result);
        }
    }

    private static void CheckResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }
}
