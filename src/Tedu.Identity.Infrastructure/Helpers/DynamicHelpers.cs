namespace Tedu.Identity.Infrastructure.Helpers;

public static class DynamicHelpers
{
    public static T Convert<T>(dynamic value)
    {
        return System.Convert.ChangeType(value, typeof(T));
    }
}
