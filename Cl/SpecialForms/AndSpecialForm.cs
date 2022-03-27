using Cl.Core;
using Cl.Core.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    public class AndSpecialForm : TaggedSpecialForm
    {
        public AndSpecialForm(ClObj cdr) : base(ClSymbol.And, cdr) { }

        public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
            .AggregateWhile<ClObj, IContext>(
                ctx.FromValue(ClBool.True),
                (ctx, expr) => expr.Reduce(ctx),
                ctx => ctx.Value != ClCell.Nil && ctx.Value != ClBool.False);
    }
}
