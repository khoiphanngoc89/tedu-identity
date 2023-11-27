using Microsoft.AspNetCore.Mvc;
using Tedu.Identity.Infrastructure;
using Tedu.Identity.Infrastructure.Const;

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
    public async Task<IActionResult> GetPermission(string roleId, CancellationToken cancellationToken = default)
    {
        var results = await this.repository.PermissionRepository.GetAllByRoleAsync(roleId, cancellationToken);
        return Ok(results);
    }
}
