using System;
using System.Collections.Generic;

namespace Cl.Extensions
{
    public static class IEnumerableOps
    {
        public static S ReduceWhile<E, S>(this IEnumerable<E> @this, S seed,Func<S, E, S> func, Predicate<S> test)
        {
            var acc = seed;
            foreach (var item in @this)
            {
                acc = func(seed, item);
                if (!test(acc)) break;
            }
            return acc;
        }

        public static void ForEach<A>(this IEnumerable<A> @this, Action<A> apply)
        {
            foreach (var item in @this) apply(item);
        }

        public static IEnumerable<(A First, B Second)> ZipIfBalanced<A, B>(this IEnumerable<A> @this, IEnumerable<B> that)
        {
            var first = @this.GetEnumerator();
            var second = that.GetEnumerator();
            while (true)
            {
                var firstIsExhausted = !first.MoveNext();
                var secondIsExhausted = !second.MoveNext();
                if (firstIsExhausted && secondIsExhausted) yield break;
                if (!firstIsExhausted && !secondIsExhausted)
                {
                    yield return (first.Current, second.Current);
                    continue;
                }
                throw new InvalidOperationException("Unbalanced");
            }
        }

        public static T FirstOrLastOrDefault<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            var current = default(T);
            foreach (var item in @this)
            {
                current = item;
                if (predicate(current)) break;
            }
            return current;
        }
    }
}
