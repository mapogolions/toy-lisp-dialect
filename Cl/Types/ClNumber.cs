namespace Cl.Types
{
    public class ClNumber<T> : ClAtom<T> where T : struct, IComparable<T>
    {
        public ClNumber(T number) : base(number) { }

        public static explicit operator ClString(ClNumber<T> obj) => new(obj.Value.ToString()!);
     }
}
