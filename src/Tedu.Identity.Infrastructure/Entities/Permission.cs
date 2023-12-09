using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Tedu.Identity.Infrastructure.Domains;

namespace Tedu.Identity.Infrastructure.Entities;

public class Permission : EntityBase<long>
{
    public string? Function { get; set; }
    public string RoleId { get; set; }
    public string? Command { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual IdentityRole? Role { get; }

    public Permission(string function, string command, string roleId)
    {
        this.Function = function?.ToUpper();
        this.Command = command?.ToUpper();
        this.RoleId = roleId;
    }
}
