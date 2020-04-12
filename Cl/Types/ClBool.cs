namespace Cl.Types
{
    public class ClBool : ClAtom<bool>
    {
        private ClBool(bool flag) : base(flag) { }

        public static ClBool True = new ClBool(true);
        public static ClBool False = new ClBool(false);

        public static ClBool Of(bool flag) => flag ? ClBool.True : ClBool.False;

        public override string ToString() => Value ? "#t" : "#f";
    }
}
