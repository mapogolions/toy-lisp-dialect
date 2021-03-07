using System;

namespace Cl.Types
{
    public abstract class ClAtom<T> : ClObj, IEquatable<ClAtom<T>>, IComparable<ClAtom<T>>
        where T : IComparable<T>
    {
        public ClAtom(T value) => Value = value;

        public T Value { get; }

        public override int CompareTo(ClObj other) =>
            other is ClAtom<T> atom ? CompareTo(atom) : throw new NotImplementedException();

        public int CompareTo(ClAtom<T> other) => Value.CompareTo(other.Value);

        public bool Equals(ClAtom<T> other) => Value.Equals(other.Value);

        public override bool Equals(ClObj other) => Equals((object) other);

        public override bool Equals(object other) => other is ClAtom<T> atom ? Equals(atom) : false;

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
    }
}
