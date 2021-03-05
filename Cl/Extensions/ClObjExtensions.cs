using Cl.Errors;
using Cl.Types;

namespace Cl.Extensions
{
    public static class ClObjExtensions
    {
        public static T Cast<T>(this ClObj @this) where T : ClObj
        {
            if (@this is T t) return t;
            throw new TypeError($"Expected {nameof(T)}, but found {@this.GetType().Name}");
        }
    }
}
