using System;
using Cl.Types;
using System.Collections.Generic;

namespace Cl.Extensions
{
    public static class EnumerableOps
    {
        public static ClCell ListOf(this IEnumerable<ClObj> @this) => BuiltIn.ListOf(@this);

        public static S AggregateWhile<E, S>(this IEnumerable<E> @this, S seed, Func<S, E, S> func, Predicate<S> test)
        {
            var acc = seed;
            foreach (var el in @this)
            {
                acc = func(acc, el);
                if (!test(acc)) break;
            }
            return acc;
        }

        public static void ForEach<A>(this IEnumerable<A> @this, Action<A> apply)
        {
            foreach (var el in @this) apply(el);
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
    }
}
