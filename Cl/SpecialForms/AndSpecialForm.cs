using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class AndSpecialForm : BaseSpecialForm
    {
        internal AndSpecialForm(IClObj cdr) : base(ClSymbol.And, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            return BuiltIn.Seq(Cdr)
                .ReduceWhile<IContext, IClObj>(
                    ctx.FromResult(ClBool.True),
                    (ctx, x) => x.Reduce(ctx),
                    ctx => ctx.Result != Nil.Given && ctx.Result != ClBool.False);
        }
    }
}
