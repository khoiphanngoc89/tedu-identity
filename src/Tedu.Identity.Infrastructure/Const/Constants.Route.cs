using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedu.Identity.Infrastructure.Const;

public static partial class SystemConstants
{
    public static class Routes
    {
        public const string DefaultApi = "/api/[controller]";
        public const string PermissionApi = "/api/[controller]/roles/{roleId}";
    }
}
