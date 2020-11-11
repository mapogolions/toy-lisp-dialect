using System;

namespace Cl.Types
{
    public abstract class ClAtom<T> : ClObj, IEquatable<ClAtom<T>>
    {
        public ClAtom(T value) => Value = value;

        public T Value { get; }

        public bool Equals(ClAtom<T> that)
        {
            if (that is ClAtom<T>) return Value.Equals(that.Value);
            return object.ReferenceEquals(this, that);
        }

        public override bool Equals(object that) => Equals(that as ClAtom<T>);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
    }
}
