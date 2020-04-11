using System;
using System.Collections.Generic;

namespace Cl.Extensions
{
    public static class IEnumerableOps
    {
        public static void ForEach<A>(this IEnumerable<A> it, Action<A> apply)
        {
            foreach (var item in it) apply(item);
        }

        public static IEnumerable<(A First, B Second)> BalancedZip<A, B>(this IEnumerable<A> it, IEnumerable<B> that)
        {
            var first = it.GetEnumerator();
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
