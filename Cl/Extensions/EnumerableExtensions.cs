using System;
using System.Collections.Generic;

namespace Cl.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> apply)
        {
            foreach (var item in self)
                apply(item);
        }
    }
}
