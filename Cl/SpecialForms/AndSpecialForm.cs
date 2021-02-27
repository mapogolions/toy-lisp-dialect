using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class AndSpecialForm : TaggedSpecialForm
    {
        internal AndSpecialForm(ClObj cdr) : base(ClSymbol.And, cdr) { }

        public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
            .AggregateWhile<ClObj, IContext>(
                ctx.FromValue(ClBool.True),
                (ctx, expr) => expr.Reduce(ctx),
                ctx => ctx.Value != ClCell.Nil && ctx.Value != ClBool.False);
    }
}
