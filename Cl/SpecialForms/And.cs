using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms;

public class And(ClObj cdr) : Tagged(ClSymbol.And, cdr)
{
    public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
        .AggregateWhile<ClObj, IContext>(
            ctx.FromValue(ClBool.True),
            (ctx, expr) => expr.Reduce(ctx),
            ctx => ctx.Value != Nil && ctx.Value != ClBool.False);
}
