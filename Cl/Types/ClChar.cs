namespace Cl.Types;

public class ClChar(char ch) : ClAtom<char>(ch)
{
    public static explicit operator ClInt(ClChar ch) => new((int) ch.Value);
}
