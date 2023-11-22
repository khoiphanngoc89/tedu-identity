using Tedu.Identity.IDP.Enities.Domains;

namespace Tedu.Identity.IDP.ViewModels;

public class PermissionViewModel : EntityBase<long>
{
    public string? RoleId { get; set; }
    public string? Function { get; set; }
    public string? Command { get; set; }
}
