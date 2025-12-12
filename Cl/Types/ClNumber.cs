namespace Cl.Types;

public class ClNumber<T>(T number) : ClAtom<T>(number) where T : struct, IComparable<T>
{
    public static explicit operator ClString(ClNumber<T> obj) => new(obj.Value.ToString()!);
 }
