using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.Json;
using Tedu.Identity.IDP.Utilities;
using Tedu.Identity.Infrastructure;
using Tedu.Identity.Infrastructure.Const;
using Tedu.Identity.Infrastructure.Entities;

namespace Tedu.Identity.IDP.Services;

public sealed class IdentityProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<User> _claimFactory;
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager repositoryManage;

    public IdentityProfileService(IUserClaimsPrincipalFactory<User> claimFactory,
                                  UserManager<User> userManager,
                                  IRepositoryManager repositoryManager)
    {
        _claimFactory = claimFactory;
        _userManager = userManager;
        this.repositoryManage = repositoryManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        if (user is null)
        {
            throw new ArgumentException(nameof(user));
        }

        var principal = await _claimFactory.CreateAsync(user);
        var claims = principal.Claims.ToList();
        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await this.repositoryManage.Permission.GetPermissionByUserAsync(user);
        var permissionClaims = permissions.Select(x => PermissionHelpers.GetPermission(x.Function, x.Command));


        claims.Add(new(SystemConstants.ConfigureOptions.FirstName, user.FirstName));
        claims.Add(new(SystemConstants.ConfigureOptions.LastName, user.LastName));
        claims.Add(new(SystemConstants.ConfigureOptions.UserName, user.UserName!));
        claims.Add(new(SystemConstants.ConfigureOptions.UserId, user.Id));
        claims.Add(new(ClaimTypes.Name, user.UserName!));
        claims.Add(new(ClaimTypes.Email, user.Email!));
        claims.Add(new(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new(SystemConstants.ConfigureOptions.FirstName, user.FirstName));
        claims.Add(new(SystemConstants.ConfigureOptions.Roles, string.Join(";", roles)));
        claims.Add(new(SystemConstants.ConfigureOptions.Permissions, JsonSerializer.Serialize(permissionClaims)));
        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user is not null;
    }
}
