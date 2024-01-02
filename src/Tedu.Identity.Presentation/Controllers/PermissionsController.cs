using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tedu.Identity.Infrastructure;
using Tedu.Identity.Infrastructure.Const;
using Tedu.Identity.Infrastructure.Models;

namespace Tedu.Identity.Presentation.Controllers;

[ApiController]
[Route(SystemConstants.Routes.PermissionApi)]
[Authorize(IdentityServerAuthenticationDefaults.AuthenticationScheme)]
public sealed class PermissionsController : ControllerBase
{
    private readonly IRepositoryManager repository;
    public PermissionsController(IRepositoryManager repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PermissionResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPermission(string roleId, CancellationToken cancellationToken = default)
    {
        var results = await this.repository.Permission.GetAllByRoleAsync(roleId, cancellationToken);
        return this.Ok(results);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PermissionAddingRequest), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreatePermission(string roleId, [FromBody] PermissionAddingRequest model, CancellationToken cancellationToken = default)
    {
        var result = await this.repository.Permission.CreatePermissionAsync(roleId, model, cancellationToken);
        return result is null ? this.NoContent() : this.Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePermission(string roleId, [FromQuery] string function, [FromQuery] string command, CancellationToken cancellationToken = default)
    {
        await this.repository.Permission.DeletePermissionAsync(roleId, function, command, cancellationToken);
        return NoContent();
    }

    [HttpPatch("update-permissions")]
    [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdatePermission(string roleId, [FromBody] IEnumerable<PermissionAddingRequest> permissions, CancellationToken cancellationToken = default)
    {
        await this.repository.Permission.UpdatePermissionsByRoleIdAsync(roleId, permissions, cancellationToken);
        return NoContent();
    }
}
