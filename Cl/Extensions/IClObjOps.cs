using Cl.Types;

namespace Cl.Extensions
{
    public static class IClObjOps
    {
        public static bool IsSelfEvaluating(this IClObj expr)
        {
            if (expr is ClCell cell) return cell == Nil.Given;
            return !(expr is ClSymbol);
        }
        public static bool IsVariable(this IClObj expr) => expr.IsSymbol();
        public static bool IsSymbol(this IClObj expr) => expr is ClSymbol;
        public static bool IsTaggedList(this IClObj expr, ClSymbol tag) => expr is ClCell cell && cell.Car == tag;
        public static bool IsQuoted(this IClObj expr) => expr.IsTaggedList(ClSymbol.Quote);
        public static bool IsAssignment(this IClObj expr) => expr.IsTaggedList(ClSymbol.Set);
        public static bool IsDefinition(this IClObj expr) => expr.IsTaggedList(ClSymbol.Define);
        public static bool IsIfThenElse(this IClObj expr) => expr.IsTaggedList(ClSymbol.IfThenElse);
        public static bool IsAnd(this IClObj expr) => expr.IsTaggedList(ClSymbol.And);
        public static bool IsOr(this IClObj expr) => expr.IsTaggedList(ClSymbol.Or);
        public static bool IsLet(this IClObj expr) => expr.IsTaggedList(ClSymbol.Let);
        public static bool IsCond(this IClObj expr) => expr.IsTaggedList(ClSymbol.Cond);
        public static bool IsBegin(this IClObj expr) => expr.IsTaggedList(ClSymbol.Begin);
        public static bool IsLambda(this IClObj expr) => expr.IsTaggedList(ClSymbol.Lambda);
        public static bool IsEmptyList(this IClObj expr) => expr == Nil.Given;
        public static T TypeOf<T>(this IClObj expr) => expr is T obj ? obj : default;
        public static T Cast<T>(this IClObj expr) => (T) expr;
    }
}
