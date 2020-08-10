using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class DefineSpecialForm : BaseSpecialForm
    {
        internal DefineSpecialForm(IClObj cdr) : base(ClSymbol.Define, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var identifier = BuiltIn.First(Cdr).Cast<ClSymbol>();
            var newCtx = BuiltIn.Second(Cdr).Reduce(ctx);
            newCtx.Env.Bind(identifier, newCtx.Value);
            return newCtx.FromResult(Nil.Given);
        }
    }
}
