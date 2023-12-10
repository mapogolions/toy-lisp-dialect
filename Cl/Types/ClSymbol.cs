using Cl.Extensions;

namespace Cl.Types
{
    public class ClSymbol : ClAtom<string>
    {
        public ClSymbol(string name) : base(name) { }

        public override IContext Reduce(IContext ctx)
        {
            if (this.Equals(Nil)) return new Context(ctx.Env);
            var value = ctx.Env.Lookup(this);
            return ctx.FromValue(value);
        }

        public static readonly ClSymbol And = new ClSymbol("and");
        public static readonly ClSymbol If = new ClSymbol("if");
        public static readonly ClSymbol Else = new ClSymbol("else");
        public static readonly ClSymbol Or = new ClSymbol("or");
        public static readonly ClSymbol Let = new ClSymbol("let");
        public static readonly ClSymbol Cond = new ClSymbol("cond");
        public static readonly ClSymbol Begin = new ClSymbol("begin");
        public static readonly ClSymbol Set = new ClSymbol("set!");
        public static readonly ClSymbol Define = new ClSymbol("define");
        public static readonly ClSymbol Quote = new ClSymbol("quote");
        public static readonly ClSymbol Lambda = new ClSymbol("lambda");
        public static readonly ClSymbol Defun = new ClSymbol("defun");
        public static readonly ClSymbol Nil = new ClSymbol("nil");
    }
}
