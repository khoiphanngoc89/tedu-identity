using Microsoft.AspNetCore.Mvc;
using Tedu.Identity.Infrastructure;
using Tedu.Identity.Infrastructure.Const;

namespace Tedu.Identity.Presentation.Controllers;

[ApiController]
[Route(SystemConstants.Routes.DefaultApi)]
public sealed class PermissionController : ControllerBase
{
    private readonly IRepositoryManager repository;
    public PermissionController(IRepositoryManager repository)
    {
        this.repository = repository;
    }
}
