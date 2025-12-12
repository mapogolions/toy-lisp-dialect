using Cl.Extensions;

namespace Cl.Types;

public class ClSymbol(string name) : ClAtom<string>(name)
{
    public override IContext Reduce(IContext ctx)
    {
        if (Equals(Nil)) return new Context(ctx.Env);
        var value = ctx.Env.Lookup(this);
        return ctx.FromValue(value);
    }

    public static readonly ClSymbol And = new("and");
    public static readonly ClSymbol If = new("if");
    public static readonly ClSymbol Else = new("else");
    public static readonly ClSymbol Or = new("or");
    public static readonly ClSymbol Let = new("let");
    public static readonly ClSymbol Cond = new("cond");
    public static readonly ClSymbol Begin = new("begin");
    public static readonly ClSymbol Set = new("set!");
    public static readonly ClSymbol Define = new("define");
    public static readonly ClSymbol Quote = new("quote");
    public static readonly ClSymbol Lambda = new("lambda");
    public static readonly ClSymbol Defun = new("defun");
    public static readonly ClSymbol Nil = new("nil");
    public static readonly ClSymbol Anonymous = new("anonymous");
    public static readonly ClSymbol Read = new("read");
}
