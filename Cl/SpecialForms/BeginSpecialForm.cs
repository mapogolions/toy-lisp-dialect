using System.Linq;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class BeginSpecialForm : TaggedSpecialForm
    {
        internal BeginSpecialForm(ClObj cdr) : base(ClSymbol.Begin, cdr) { }

        public override IContext Reduce(IContext ctx) => BuiltIn.Seq(Cdr)
            .Aggregate(ctx.FromValue(ClCell.Nil), (ctx, expr) => expr.Reduce(ctx));
    }
}
