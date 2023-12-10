using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    public class DefineSpecialForm : TaggedSpecialForm
    {
        public DefineSpecialForm(ClObj cdr) : base(ClSymbol.Define, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var identifier = BuiltIn.First(Cdr).Cast<ClSymbol>();
            var (value, env) = BuiltIn.Second(Cdr).Reduce(ctx);
            env.Bind(identifier, value);
            return new Context(env);
        }
    }
}
