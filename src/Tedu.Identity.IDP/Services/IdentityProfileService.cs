using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Tedu.Identity.Infrastructure.Const;
using Tedu.Identity.Infrastructure.Enities;

namespace Tedu.Identity.IDP.Services;

public sealed class IdentityProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<User> _claimFactory;
    private readonly UserManager<User> _userManager;

    public IdentityProfileService(IUserClaimsPrincipalFactory<User> claimFactory, UserManager<User> userManager)
    {
        _claimFactory = claimFactory;
        _userManager = userManager;
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

        claims.Add(new(SystemConstants.Claims.FirstName, user.FirstName));
        claims.Add(new(SystemConstants.Claims.LastName, user.LastName));
        claims.Add(new(SystemConstants.Claims.UserName, user.UserName));
        claims.Add(new(SystemConstants.Claims.UserId, user.Id));
        claims.Add(new(ClaimTypes.Name, user.UserName));
        claims.Add(new(ClaimTypes.Email, user.Email));
        claims.Add(new(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new(SystemConstants.Claims.FirstName, user.FirstName));
        claims.Add(new(SystemConstants.Claims.Roles, string.Join(";", roles)));
        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user is not null;
    }
}
