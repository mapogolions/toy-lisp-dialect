using Cl.Contracts;
using Cl.Errors;
using Cl.Extensions;

namespace Cl.Types
{
    public class ClObj : IReducable
    {
        public virtual IContext Reduce(IContext ctx) => ctx.FromValue(this);

        public static ClObj operator +(ClObj @this, ClObj that) =>
            (@this, that) switch
            {
                (ClInt fixnum, _) => fixnum + that,
                (ClDouble floatingPoint, _) => floatingPoint + that,
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {@this.GetType().Name}")
            };

        public static ClObj operator *(ClObj @this, ClObj that) =>
            (@this, that) switch
            {
                (ClInt fixnum, _) => fixnum * that,
                (ClDouble floatingPoint, _) => floatingPoint * that,
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {@this.GetType().Name}")
            };

        public static ClObj operator /(ClObj @this, ClObj that) =>
            (@this, that) switch
            {
                (ClInt fixnum, _) => fixnum / that,
                (ClDouble floatingPoint, _) => floatingPoint / that,
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {@this.GetType().Name}")
            };
    }
}
