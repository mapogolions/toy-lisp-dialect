using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class OrSpecialForm : BaseSpecialForm
    {
        internal OrSpecialForm(IClObj cdr) : base(ClSymbol.Or, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            return BuiltIn.Seq(Cdr)
                .ReduceWhile<IContext, IClObj>(
                    ctx.FromResult(ClBool.False),
                    (ctx, x) => x.Reduce(ctx),
                    ctx => ctx.Result == Nil.Given || ctx.Result == ClBool.False);
        }
    }
}
