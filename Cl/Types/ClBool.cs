namespace Cl.Types
{
    public class ClBool : ClAtom<bool>
    {
        private ClBool(bool flag) : base(flag) { }

        public static ClBool True = new ClBool(true);
        public static ClBool False = new ClBool(false);
    }
}
