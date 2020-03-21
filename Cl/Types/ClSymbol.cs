namespace Cl.Types
{
    public class ClSymbol : ClAtom<string>
    {
        public ClSymbol(string name) : base(name) { }

        public static ClSymbol And = new ClSymbol("and");
        public static ClSymbol IfThenElse = new ClSymbol("if");
        public static ClSymbol Or = new ClSymbol("or");
        public static ClSymbol Let = new ClSymbol("let");
        public static ClSymbol Cond = new ClSymbol("cond");
        public static ClSymbol Begin = new ClSymbol("begin");
        public static ClSymbol Set = new ClSymbol("set!");
        public static ClSymbol Define = new ClSymbol("define");
        public static ClSymbol Quote = new ClSymbol("quote");
        public static ClSymbol Lambda = new ClSymbol("lambda");
    }
}
