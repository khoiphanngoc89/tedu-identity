using Tedu.Identity.Infrastructure.Entities;

namespace Tedu.Identity.Infrastructure.Models.Mappings;

public static class PermissionMapping
{
    public static PermissionUserResponse ToPermissionUserResponse(this Permission permission)
        => new(permission.Function!, permission.Command!);
}
