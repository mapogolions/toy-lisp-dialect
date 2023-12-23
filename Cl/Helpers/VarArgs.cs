using Cl.Errors;
using Cl.Types;

namespace Cl.Helpers
{
    public static class VarArgs
    {
        public static T Get<T>(params ClObj[] items) =>
            items is null ? throw new ArgumentNullException(nameof(items)) : Get<T>(items.AsSpan());

        public static T Get<T>(Span<ClObj> items)
        {
            if (items.Length != 1)
            {
                throw new TypeError($"Arity exception: function expects 1 arg, but passed {items.Length}");
            }
            if (items[0] is not T t)
            {
                throw new TypeError($"Expected {typeof(T).Name}, but found {items[0].GetType().Name}");
            }
            return t;
        }

        public static (T1, T2) Get<T1, T2>(params ClObj[] items) =>
            items is null ? throw new ArgumentNullException(nameof(items)) : Get<T1, T2>(items.AsSpan());

        public static (T1, T2) Get<T1, T2>(Span<ClObj> items)
        {
            if (items.Length != 2)
            {
                throw new TypeError($"Arity exception: function expects 2 args, but passed {items.Length}");
            }
            var t1 = Get<T1>(items[0..1]);
            if (items[1] is not T2 t2)
            {
                throw new TypeError($"Expected {typeof(T2).Name}, but found {items[1].GetType().Name}");
            }
            return (t1, t2);
        }

        public static (T1, T2, T3) Get<T1, T2, T3>(params ClObj[] items) =>

            items is null ? throw new ArgumentNullException(nameof(items)) : Get<T1, T2, T3>(items.AsSpan());

        public static (T1, T2, T3) Get<T1, T2, T3>(Span<ClObj> items)
        {
            if (items.Length != 3)
            {
                throw new TypeError($"Arity exception: function expects 3 args, but passed {items.Length}");
            }
            var (t1, t2) = Get<T1, T2>(items[0..2]);
            if (items[2] is not T3 t3)
            {
                throw new TypeError($"Expected {typeof(T3).Name}, but found {items[2].GetType().Name}");
            }
            return (t1, t2, t3);
        }

        public static (T1, T2, T3, T4) Get<T1, T2, T3, T4>(params ClObj[] items) =>

            items is null ? throw new ArgumentNullException(nameof(items)) : Get<T1, T2, T3, T4>(items.AsSpan());

        public static (T1, T2, T3, T4) Get<T1, T2, T3, T4>(Span<ClObj> items)
        {
            if (items.Length != 4)
            {
                throw new TypeError($"Arity exception: function expectes 4 args, but passed {items.Length}");
            }
            var (t1, t2, t3) = Get<T1, T2, T3>(items[0..3]);
            if (items[3] is not T4 t4)
            {
                throw new TypeError($"Expected {typeof(T4).Name}, but found {items[3].GetType().Name}");
            }
            return (t1, t2, t3, t4);
        }
    }
}
