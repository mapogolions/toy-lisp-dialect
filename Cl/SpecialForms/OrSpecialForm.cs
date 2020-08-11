using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class OrSpecialForm : TaggedSpecialForm
    {
        internal OrSpecialForm(IClObj cdr) : base(ClSymbol.Or, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            return BuiltIn.Seq(Cdr)
                .ReduceWhile<IClObj, IContext>(
                    ctx.FromResult(ClBool.False),
                    (ctx, x) => x.Reduce(ctx),
                    ctx => ctx.Value == Nil.Given || ctx.Value == ClBool.False);
        }
    }
}
