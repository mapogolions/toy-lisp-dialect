using System;
using Cl.Types;

namespace Cl.Extensions
{
    public static class ClObjExtensions
    {
        public static T TypeOf<T>(this ClObj @this) => @this is T obj ? obj : default;
        public static T Cast<T>(this ClObj @this) where T : ClObj => (T) @this;
        public static T CastOrThrow<T>(this ClObj @this, string message) =>
            TypeOf<T>(@this) ?? throw new InvalidOperationException(message);
    }
}
