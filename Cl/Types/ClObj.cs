using System;
using Cl.Errors;
using Cl.Extensions;

namespace Cl.Types
{
    public class ClObj : IReducable, IEquatable<ClObj>, IComparable<ClObj>
    {
        public virtual bool Equals(ClObj other) => ReferenceEquals(this, other);

        public override bool Equals(object other) => other is ClObj obj ? Equals(obj) : false;

        public virtual IContext Reduce(IContext ctx) => ctx.FromValue(this);

        public static ClObj operator +(ClObj @this, ClObj other) =>
            (@this, other) switch
            {
                (ClInt fixnum, _) => fixnum + other,
                (ClDouble floatingPoint, _) => floatingPoint + other,
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {@this.GetType().Name}")
            };

        public static ClObj operator *(ClObj @this, ClObj other) =>
            (@this, other) switch
            {
                (ClInt fixnum, _) => fixnum * other,
                (ClDouble floatingPoint, _) => floatingPoint * other,
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {@this.GetType().Name}")
            };

        public static ClObj operator /(ClObj @this, ClObj other) =>
            (@this, other) switch
            {
                (ClInt fixnum, _) => fixnum / other,
                (ClDouble floatingPoint, _) => floatingPoint / other,
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {@this.GetType().Name}")
            };

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public virtual int CompareTo(ClObj other) => throw new NotImplementedException();
    }
}
