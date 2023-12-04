using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tedu.Identity.Infrastructure.Helpers;

public static class DynamicHelpers
{
    public static T Convert<T>(dynamic value)
    {
        return System.Convert.ChangeType(value, typeof(T));
    }
}
