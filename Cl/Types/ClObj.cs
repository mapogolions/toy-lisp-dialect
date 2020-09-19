using System;
using Cl.Contracts;

namespace Cl.Types
{
    public class ClObj : IReducable
    {
        public virtual IContext Reduce(IContext ctx) => ctx.FromResult(this);

        public static ClObj operator +(ClObj @this, ClObj that) =>
            (@this, that) switch
            {
                (ClInt fixnum, _) => fixnum + that,
                (ClDouble floatingPoint, _) => floatingPoint + that,
                _ => throw new InvalidOperationException()
            };

        public static ClObj operator *(ClObj @this, ClObj that) =>
            (@this, that) switch
            {
                (ClInt fixnum, _) => fixnum * that,
                (ClDouble floatingPoint, _) => floatingPoint * that,
                _ => throw new InvalidOperationException()
            };

        public static ClObj operator /(ClObj @this, ClObj that) =>
            (@this, that) switch
            {
                (ClInt fixnum, _) => fixnum / that,
                (ClDouble floatingPoint, _) => floatingPoint / that,
                _ => throw new InvalidOperationException()
            };
    }
}
