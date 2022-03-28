using Cl;
using Cl.Types;

namespace Cl.SpecialForms
{
    public class IfSpecialForm : TaggedSpecialForm
    {
        public IfSpecialForm(ClObj cdr) : base(ClSymbol.Set, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var newCtx = BuiltIn.First(Cdr).Reduce(ctx);
            if (newCtx.Value != ClCell.Nil && newCtx.Value != ClBool.False)
                return BuiltIn.Second(Cdr).Reduce(newCtx);
            var elseBranch = BuiltIn.Cddr(Cdr);
            return elseBranch ==  ClCell.Nil
                ? new Context(newCtx.Env) : BuiltIn.First(elseBranch).Reduce(newCtx);
        }
    }
}
