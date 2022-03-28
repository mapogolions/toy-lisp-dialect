using System;
using Cl.Errors;
using Cl.Types;

namespace Cl.Helpers
{
    public static class ArrayHelpers
    {
        public static T Unpack<T>(ClObj[] items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Length != 1)
            {
                throw new TypeError($"Arity exception: function expects 1 arg, but passed {items.Length}");
            }
            if (!(items[0] is T t))
            {
                throw new TypeError($"Expected {typeof(T).Name}, but found {items[0].GetType().Name}");
            }
            return t;
        }

        public static (T1, T2) Unpack<T1, T2>(ClObj[] items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Length != 2)
            {
                throw new TypeError($"Arity exception: function expects 2 args, but passed {items.Length}");
            }
            var t1 = Unpack<T1>(items.AsSpan(0, 1).ToArray());
            if (!(items[1] is T2 t2))
            {
                throw new TypeError($"Expected {typeof(T2).Name}, but found {items[1].GetType().Name}");
            }
            return (t1, t2);
        }

        public static (T1, T2, T3) Unpack<T1, T2, T3>(ClObj[] items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Length != 3)
            {
                throw new TypeError($"Arity exception: function expects 3 args, but passed {items.Length}");
            }
            var (t1, t2) = Unpack<T1, T2>(items.AsSpan(0, 2).ToArray());
            if (!(items[2] is T3 t3))
            {
                throw new TypeError($"Expected {typeof(T3).Name}, but found {items[2].GetType().Name}");
            }
            return (t1, t2, t3);
        }

        public static (T1, T2, T3, T4) Unpack<T1, T2, T3, T4>(ClObj[] items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Length != 4)
            {
                throw new TypeError($"Arity exception: function expectes 4 args, but passed {items.Length}");
            }
            var (t1, t2, t3) = Unpack<T1, T2, T3>(items.AsSpan(0, 3).ToArray());
            if (!(items[3] is T4 t4))
            {
                throw new TypeError($"Expected {typeof(T4).Name}, but found {items[3].GetType().Name}");
            }
            return (t1, t2, t3, t4);
        }
    }
}
