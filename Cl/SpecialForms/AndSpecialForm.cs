using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class AndSpecialForm : TaggedSpecialForm
    {
        internal AndSpecialForm(IClObj cdr) : base(ClSymbol.And, cdr) { }

        public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
            .AggregateWhile<IClObj, IContext>(
                ctx.FromResult(ClBool.True),
                (ctx, expr) => expr.Reduce(ctx),
                ctx => ctx.Value != Nil.Given && ctx.Value != ClBool.False);
    }
}
