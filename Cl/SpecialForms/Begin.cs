using Cl.Types;

namespace Cl.SpecialForms;

public class Begin(ClObj cdr) : Tagged(ClSymbol.Begin, cdr)
{
    public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
        .Aggregate<ClObj, IContext>(new Context(ctx.Env), (ctx, expr) => expr.Reduce(ctx));
}
