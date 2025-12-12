using Cl.Types;

namespace Cl.SpecialForms;

public class If(ClObj cdr) : Tagged(ClSymbol.Set, cdr)
{
    public override IContext Reduce(IContext ctx)
    {
        var newCtx = BuiltIn.First(Cdr).Reduce(ctx);
        if (newCtx.Value != Nil && newCtx.Value != ClBool.False)
            return BuiltIn.Second(Cdr).Reduce(newCtx);
        var elseBranch = BuiltIn.Cddr(Cdr);
        return elseBranch == Nil
            ? new Context(newCtx.Env) : BuiltIn.First(elseBranch).Reduce(newCtx);
    }
}
