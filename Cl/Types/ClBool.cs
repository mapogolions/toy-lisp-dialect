namespace Cl.Types
{
    public class ClBool : ClAtom<bool>
    {
        private ClBool(bool flag) : base(flag) { }

        public static readonly ClBool True = new ClBool(true);
        public static readonly ClBool False = new ClBool(false);

        public static ClBool Of(bool flag) => flag ? ClBool.True : ClBool.False;

        public override string ToString() => Value ? "#t" : "#f";
    }
}
