using System;
using Cl.Types;

namespace Cl.Helpers
{
    public static class ArrayHelpers
    {
        public static T Unpack<T>(ClObj[] items)
        {
            if (items is null) throw new NullReferenceException(nameof(items));
            if (items.Length != 1)
                throw new InvalidOperationException("Arity exception");
            if (!(items[0] is T t))
                throw new InvalidOperationException("Unexpected type");
            return t;
        }

        public static (T1, T2) Unpack<T1, T2>(ClObj[] items)
        {
            if (items is null) throw new NullReferenceException(nameof(items));
            if (items.Length != 2)
                throw new InvalidOperationException("Arity exception");
            var t1 = Unpack<T1>(items.AsSpan(0, 1).ToArray());
            if (!(items[1] is T2 t2))
                throw new InvalidOperationException("Unexpected type");
            return (t1, t2);
        }

        public static (T1, T2, T3) Unpack<T1, T2, T3>(ClObj[] items)
        {
            if (items is null) throw new NullReferenceException(nameof(items));
            if (items.Length != 3)
                throw new InvalidOperationException("Arity exception");
            var (t1, t2) = Unpack<T1, T2>(items.AsSpan(0, 2).ToArray());
            if (!(items[2] is T3 t3))
                throw new InvalidOperationException("Unexpected type");
            return (t1, t2, t3);
        }

        public static (T1, T2, T3, T4) Unpack<T1, T2, T3, T4>(ClObj[] items)
        {
            if (items is null) throw new NullReferenceException(nameof(items));
            if (items.Length != 4)
                throw new InvalidOperationException("Arity exception");
            var (t1, t2, t3) = Unpack<T1, T2, T3>(items.AsSpan(0, 3).ToArray());
            if (!(items[3] is T4 t4))
                throw new InvalidOperationException("Unexpected type");
            return (t1, t2, t3, t4);
        }
    }
}
