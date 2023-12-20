using System.ComponentModel.DataAnnotations;

namespace Tedu.Identity.Infrastructure.Models;

public class PermissionAddingRequest
{
    [Required]
    public string? Function { get; set; }
    [Required]
    public string? Command { get; set; }
}
