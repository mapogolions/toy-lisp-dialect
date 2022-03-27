using Cl.Core;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    public class OrSpecialForm : TaggedSpecialForm
    {
        public OrSpecialForm(ClObj cdr) : base(ClSymbol.Or, cdr) { }

        public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
            .AggregateWhile<ClObj, IContext>(
                ctx.FromValue(ClBool.False),
                (ctx, expr) => expr.Reduce(ctx),
                ctx => ctx.Value == ClCell.Nil || ctx.Value == ClBool.False);
    }
}
