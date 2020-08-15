using Cl.Contracts;

namespace Cl.Types
{
    public class ClSymbol : ClAtom<string>
    {
        public ClSymbol(string name) : base(name) { }

         public override IContext Reduce(IContext ctx)
        {
            var result = ctx.Env.Lookup(this);
            return ctx.FromResult(result);
        }

        public static ClSymbol And = new ClSymbol("and");
        public static ClSymbol If = new ClSymbol("if");
        public static ClSymbol Else = new ClSymbol("else");
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

