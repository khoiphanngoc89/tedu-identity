using Microsoft.AspNetCore.Identity;
using Tedu.Identity.IDP.Enities.Domains;

namespace Tedu.Identity.IDP.Enities;

public class Permission : EntityBase<long>
{
    public string? Function { get; set; }
    public string RoleId { get; set; } = string.Empty;
    public string? Command { get; set; }
    public virtual IdentityRole? Role { get; set; }

    public Permission(string function, string command, string roleId)
    {
        this.Function = function;
        this.Command = command;
        this.RoleId = roleId;
    }
}
