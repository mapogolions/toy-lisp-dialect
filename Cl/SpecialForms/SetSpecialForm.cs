using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class SetSpecialForm : TaggedSpecialForm
    {
        internal SetSpecialForm(IClObj cdr) : base(ClSymbol.Set, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var identifier = BuiltIn.First(Cdr).Cast<ClSymbol>();
            var newCtx = BuiltIn.Second(Cdr).Reduce(ctx);
            newCtx.Env.Assign(identifier, newCtx.Value);
            return newCtx.FromResult(ClCell.Nil);
        }
    }
}
