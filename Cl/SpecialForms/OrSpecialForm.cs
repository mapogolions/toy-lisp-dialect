using Cl.Contracts;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class OrSpecialForm : BaseSpecialForm
    {
        internal OrSpecialForm(IClObj cdr) : base(ClSymbol.Or, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var tail = Cdr;
            while (tail != Nil.Given)
            {
                ctx = BuiltIn.Head(tail).Reduce(ctx);
                if (ctx.Result != Nil.Given && ctx.Result != ClBool.False)
                    return ctx.FromResult(ClBool.True);
                tail = BuiltIn.Tail(tail);
            }
            return ctx.FromResult(ClBool.False);
        }
    }
}
