using System;
using Cl.Types;

namespace Cl.Extensions
{
    public static class IClObjOps
    {
        public static bool IsSelfEvaluating(this IClObj @this)
        {
            if (@this is ClCell cell) return cell == Nil.Given;
            return !(@this is ClSymbol);
        }
        public static bool IsVariable(this IClObj @this) => @this.IsSymbol();
        public static bool IsSymbol(this IClObj @this) => @this is ClSymbol;
        public static bool IsTaggedList(this IClObj @this, ClSymbol tag) => @this is ClCell cell && cell.Car.Equals(tag);
        public static bool IsQuoted(this IClObj @this) => @this.IsTaggedList(ClSymbol.Quote);
        public static bool IsAssignment(this IClObj @this) => @this.IsTaggedList(ClSymbol.Set);
        public static bool IsDefinition(this IClObj @this) => @this.IsTaggedList(ClSymbol.Define);
        public static bool IsIf(this IClObj @this) => @this.IsTaggedList(ClSymbol.If);
        public static bool IsAnd(this IClObj @this) => @this.IsTaggedList(ClSymbol.And);
        public static bool IsOr(this IClObj @this) => @this.IsTaggedList(ClSymbol.Or);
        public static bool IsLet(this IClObj @this) => @this.IsTaggedList(ClSymbol.Let);
        public static bool IsCond(this IClObj @this) => @this.IsTaggedList(ClSymbol.Cond);
        public static bool IsBegin(this IClObj @this) => @this.IsTaggedList(ClSymbol.Begin);
        public static bool IsLambda(this IClObj @this) => @this.IsTaggedList(ClSymbol.Lambda);
        public static bool IsEmptyList(this IClObj @this) => @this == Nil.Given;
        public static T TypeOf<T>(this IClObj @this) => @this is T obj ? obj : default;
        public static T Cast<T>(this IClObj @this) => (T) @this;
        public static T CastOrThrow<T>(this IClObj @this, string message) =>
            TypeOf<T>(@this) ?? throw new InvalidOperationException(message);
    }
}
