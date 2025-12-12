using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms;

public class Or(ClObj cdr) : Tagged(ClSymbol.Or, cdr)
{
    public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
        .AggregateWhile<ClObj, IContext>(
            ctx.FromValue(ClBool.False),
            (ctx, expr) => expr.Reduce(ctx),
            ctx => ctx.Value == Nil || ctx.Value == ClBool.False);
}
