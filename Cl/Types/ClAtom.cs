namespace Cl.Types
{
    public abstract class ClAtom<T> : IClObj
    {
        public ClAtom(T value) => Value = value;

        public T Value { get; }
    }
}
