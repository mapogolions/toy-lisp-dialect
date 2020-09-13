namespace Cl.Types
{
    public class ClString : ClAtom<string>
    {
        public ClString(string str) : base(str) { }

        public static explicit operator ClInt (ClString obj) => new ClInt(int.Parse(obj.Value));
        public static explicit operator ClDouble (ClString obj) => new ClDouble(double.Parse(obj.Value));
    }
}
