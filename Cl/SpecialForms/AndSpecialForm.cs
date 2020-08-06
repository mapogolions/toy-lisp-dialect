using Cl.Contracts;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class AndSpecialForm : BaseSpecialForm
    {
        internal AndSpecialForm(IClObj cdr) : base(ClSymbol.And, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var tail = Cdr;
            while (tail != Nil.Given)
            {
                ctx = BuiltIn.Head(tail).Reduce(ctx);
                if (ctx.Result == Nil.Given || ctx.Result == ClBool.False)
                    return ctx.FromResult(ClBool.False);
                tail = BuiltIn.Tail(tail);
            }
            return ctx.FromResult(ClBool.True);
        }
    }
}
