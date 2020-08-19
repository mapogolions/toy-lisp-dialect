using Cl.Contracts;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class IfSpecialForm : TaggedSpecialForm
    {
        internal IfSpecialForm(IClObj cdr) : base(ClSymbol.Set, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var newCtx = BuiltIn.First(Cdr).Reduce(ctx);
            if (newCtx.Value != ClCell.Nil && newCtx.Value != ClBool.False)
                return BuiltIn.Second(Cdr).Reduce(newCtx);
            var elseBranch = BuiltIn.Cddr(Cdr);
            return elseBranch ==  ClCell.Nil
                ? newCtx.FromResult(ClCell.Nil) : BuiltIn.First(elseBranch).Reduce(newCtx);
        }
    }
}
