namespace Cl.Types
{
    public class ClChar : ClAtom<char>
    {
        public ClChar(char ch) : base(ch) { }

        public static explicit operator ClInt(ClChar ch) => new ClInt((int) ch.Value);
    }
}
