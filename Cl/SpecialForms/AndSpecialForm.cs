using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class AndSpecialForm : TaggedSpecialForm
    {
        internal AndSpecialForm(IClObj cdr) : base(ClSymbol.And, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            return BuiltIn.Seq(Cdr)
                .ReduceWhile<IClObj, IContext>(
                    ctx.FromResult(ClBool.True),
                    (ctx, x) => x.Reduce(ctx),
                    ctx => ctx.Value != Nil.Given && ctx.Value != ClBool.False);
        }
    }
}
