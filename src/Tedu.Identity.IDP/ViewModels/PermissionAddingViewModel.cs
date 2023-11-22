using System.ComponentModel.DataAnnotations;

namespace Tedu.Identity.IDP.ViewModels;

public class PermissionAddingViewModel
{
    [Required]
    public string? Function { get; set; }
    [Required]
    public string? Command { get; set; }
}
