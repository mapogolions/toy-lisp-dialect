namespace Cl.Types
{
    public abstract class ClAtom<T> : IClObj
    {
        public T Value { get; }

        public ClAtom(T value) => Value = value;
    }
}
