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
            var currentCtx = ctx.FromResult(ClBool.False);
            while (tail != Nil.Given)
            {
                currentCtx = BuiltIn.Head(tail).Reduce(currentCtx);
                if (currentCtx.Result != Nil.Given && currentCtx.Result != ClBool.False)
                    break;
                tail = BuiltIn.Tail(tail);
            }
            return currentCtx;
        }
    }
}
