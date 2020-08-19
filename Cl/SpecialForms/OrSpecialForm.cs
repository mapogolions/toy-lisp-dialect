using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class OrSpecialForm : TaggedSpecialForm
    {
        internal OrSpecialForm(IClObj cdr) : base(ClSymbol.Or, cdr) { }

        public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
            .AggregateWhile<IClObj, IContext>(
                ctx.FromResult(ClBool.False),
                (ctx, expr) => expr.Reduce(ctx),
                ctx => ctx.Value == ClCell.Nil || ctx.Value == ClBool.False);
    }
}
