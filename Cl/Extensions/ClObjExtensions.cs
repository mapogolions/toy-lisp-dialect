using Cl.Errors;
using Cl.Types;

namespace Cl.Extensions;

public static class ClObjExtensions
{
    public static T Cast<T>(this ClObj @this) where T : ClObj
    {
        ArgumentNullException.ThrowIfNull(@this, nameof(@this));
        if (@this is T t) return t;
        throw new TypeError($"'{@this}' cannot be converted to {typeof(T).Name}");
    }
}
