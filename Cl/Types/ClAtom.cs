using System;

namespace Cl.Types
{
    public abstract class ClAtom<T> : IClObj, IEquatable<ClAtom<T>>
    {
        public ClAtom(T value) => Value = value;

        public T Value { get; }

        public bool Equals(ClAtom<T> other)
        {
            if (other is ClAtom<T>) return Value.Equals(other.Value);
            return object.ReferenceEquals(this, other);
        }

        public override bool Equals(object that) => Equals(that as ClAtom<T>);

        public override int GetHashCode() => Value.GetHashCode();
    }
}
