using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tedu.Identity.Infrastructure;
using Tedu.Identity.Infrastructure.Const;
using Tedu.Identity.Infrastructure.ViewModels;

namespace Tedu.Identity.Presentation.Controllers;

[ApiController]
[Route(SystemConstants.Routes.PermissionApi)]
public sealed class PermissionController : ControllerBase
{
    private readonly IRepositoryManager repository;
    public PermissionController(IRepositoryManager repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PermissionViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPermission(string roleId, CancellationToken cancellationToken = default)
    {
        var results = await this.repository.PermissionRepository.GetAllByRoleAsync(roleId, cancellationToken);
        return this.Ok(results);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PermissionAddingViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreatePermission(string roleId, [FromBody] PermissionAddingViewModel model, CancellationToken cancellationToken = default)
    {
        var result = await this.repository.PermissionRepository.CreatePermissionAsync(roleId, model, cancellationToken);
        return result is null ? this.NoContent() : this.Ok(result);
    }
}
