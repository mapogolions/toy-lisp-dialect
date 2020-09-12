using System;

namespace Cl.Types
{
    public class ClString : ClAtom<string>
    {
        public ClString(string str) : base(str) { }

        public static explicit operator ClInt (ClString str) => new ClInt(int.Parse(str.Value));
        public static explicit operator ClDouble (ClString str) => new ClDouble(double.Parse(str.Value));
    }
}
