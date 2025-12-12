namespace Cl.Types;

public abstract class ClAtom<T>(T value) : ClObj, IEquatable<ClAtom<T>>, IComparable<ClAtom<T>>
    where T : IComparable<T>
{
    public T Value { get; } = value;

    public override int CompareTo(ClObj? other) => CompareTo(other as ClAtom<T>);

    public int CompareTo(ClAtom<T>? other) => other is {} atom
        ? Value.CompareTo(atom.Value) : 1;

    public bool Equals(ClAtom<T>? other) => other is {} atom && Value.Equals(atom.Value);

    public override bool Equals(ClObj? other) => Equals(other as ClAtom<T>);

    public override bool Equals(object? other) => Equals(other as ClAtom<T>);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString() ?? throw new NotSupportedException();
}
