namespace Cl.Types
{
    public class ClBool : ClAtom<bool>
    {
        public static ClBool True = new ClBool(true);
        public static ClBool False = new ClBool(false);

        private ClBool(bool flag) : base(flag) { }
    }
}
