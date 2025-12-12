namespace Cl.Types;

public class ClString(string str) : ClAtom<string>(str)
{
    public static explicit operator ClInt (ClString obj) => new(int.Parse(obj.Value));
    public static explicit operator ClDouble (ClString obj) => new(double.Parse(obj.Value));
}
