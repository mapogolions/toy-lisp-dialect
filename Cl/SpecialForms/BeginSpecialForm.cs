using System.Linq;
using Cl.Contracts;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class BeginSpecialForm : TaggedSpecialForm
    {
        internal BeginSpecialForm(ClObj cdr) : base(ClSymbol.Begin, cdr) { }

        public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
            .Aggregate(ctx.FromResult(ClCell.Nil), (ctx, expr) => expr.Reduce(ctx));
    }
}
