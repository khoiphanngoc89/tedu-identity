using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedu.Identity.Infrastructure.Models;

public sealed record PermissionUserResponse(string Function, string Command)
{
}
