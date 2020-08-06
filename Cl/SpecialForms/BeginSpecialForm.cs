using Cl.Contracts;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class BeginSpecialForm : BaseSpecialForm
    {
        internal BeginSpecialForm(IClObj cdr) : base(ClSymbol.Begin, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var tail = Cdr;
            ctx = ctx.FromResult(Nil.Given);
            while (tail != Nil.Given)
            {
                ctx = BuiltIn.Head(tail).Reduce(ctx);
                tail = BuiltIn.Tail(tail);
            }
            return ctx;
        }
    }
}
