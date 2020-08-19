using System;
using Cl.Types;

namespace Cl.Extensions
{
    public static class ArrayOps
    {
        public static T Unpack<T>(this IClObj[] @this)
        {
            if (@this.Length != 1)
                throw new InvalidOperationException("Arity exception");
            if (!(@this[0] is T t))
                throw new InvalidOperationException("Unexpected type");
            return t;
        }

        public static (T1, T2) Unpack<T1, T2>(this IClObj[] @this)
        {
            if (@this.Length != 2)
                throw new InvalidOperationException("Arity exception");
            var t1 = Unpack<T1>(@this.AsSpan(0, 1).ToArray());
            if (!(@this[1] is T2 t2))
                throw new InvalidOperationException("Unexpected type");
            return (t1, t2);
        }

        public static (T1, T2, T3) Unpack<T1, T2, T3>(this IClObj[] @this)
        {
            if (@this.Length != 3)
                throw new InvalidOperationException("Arity exception");
            var (t1, t2) = Unpack<T1, T2>(@this.AsSpan(0, 2).ToArray());
            if (!(@this[2] is T3 t3))
                throw new InvalidOperationException("Unexpected type");
            return (t1, t2, t3);
        }

        public static (T1, T2, T3, T4) Unpack<T1, T2, T3, T4>(this IClObj[] @this)
        {
            if (@this.Length != 4)
                throw new InvalidOperationException("Arity exception");
            var (t1, t2, t3) = Unpack<T1, T2, T3>(@this.AsSpan(0, 3).ToArray());
            if (!(@this[3] is T4 t4))
                throw new InvalidOperationException("Unexpected type");
            return (t1, t2, t3, t4);
        }
    }
}
