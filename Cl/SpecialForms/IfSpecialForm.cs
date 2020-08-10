using Cl.Contracts;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class IfSpecialForm : BaseSpecialForm
    {
        internal IfSpecialForm(IClObj cdr) : base(ClSymbol.Set, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var newCtx = BuiltIn.First(Cdr).Reduce(ctx);
            if (newCtx.Value != Nil.Given && newCtx.Value != ClBool.False)
                return BuiltIn.Second(Cdr).Reduce(newCtx);
            var elseBranch = BuiltIn.Cddr(Cdr);
            return elseBranch ==  Nil.Given
                ? newCtx.FromResult(Nil.Given) : BuiltIn.First(elseBranch).Reduce(newCtx);
        }
    }
}
