using System;
using Cl.Types;

namespace Cl.Extensions
{
    public static class IClObjOps
    {
        public static bool IsSelfEvaluating(this IClObj @this)
        {
            if (@this is ClCell cell) return cell == ClCell.Nil;
            return !(@this is ClSymbol);
        }

        public static bool IsEmptyList(this IClObj @this) => @this == ClCell.Nil;
        public static T TypeOf<T>(this IClObj @this) => @this is T obj ? obj : default;
        public static T Cast<T>(this IClObj @this) => (T) @this;
        public static T CastOrThrow<T>(this IClObj @this, string message) =>
            TypeOf<T>(@this) ?? throw new InvalidOperationException(message);
    }
}
