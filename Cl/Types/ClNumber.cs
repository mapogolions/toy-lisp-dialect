using System;

namespace Cl.Types
{
    public class ClNumber<T> : ClAtom<T> where T : struct
    {
        public ClNumber(T number) : base(number) { }

        public static explicit operator ClString(ClNumber<T> obj) => new ClString(obj.Value.ToString());
     }
}
