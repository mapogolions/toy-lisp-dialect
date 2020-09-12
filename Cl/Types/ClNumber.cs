namespace Cl.Types
{
    public class ClNumber<T> : ClAtom<T> where T : struct
    {
        public ClNumber(T number) : base(number) { }
    }
}
