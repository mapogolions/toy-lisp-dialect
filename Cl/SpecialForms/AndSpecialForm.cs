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
            var currentCtx = ctx.FromResult(ClBool.True);
            while (tail != Nil.Given)
            {
                currentCtx = BuiltIn.Head(tail).Reduce(currentCtx);
                if (currentCtx.Result == Nil.Given || currentCtx.Result == ClBool.False)
                    break;
                tail = BuiltIn.Tail(tail);
            }
            return currentCtx;
        }
    }
}
