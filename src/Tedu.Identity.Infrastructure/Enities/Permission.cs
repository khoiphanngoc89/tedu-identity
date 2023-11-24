using Microsoft.AspNetCore.Identity;
using Tedu.Identity.Infrastructure.Domains;

namespace Tedu.Identity.Infrastructure.Enities;

public class Permission : EntityBase<long>
{
    public string? Function { get; set; }
    public string RoleId { get; set; }
    public string? Command { get; set; }
    public virtual IdentityRole? Role { get; }

    public Permission(string function, string command, string roleId)
    {
        this.Function = function?.ToUpper();
        this.Command = command?.ToUpper();
        this.RoleId = roleId;
    }
}
