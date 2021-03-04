using System.Linq;
using Cl.Contracts;
using Cl.Errors;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class LambdaSpecialForm : TaggedSpecialForm
    {
        internal LambdaSpecialForm(ClObj cdr) : base(ClSymbol.Lambda, cdr) { }

        public override IContext Reduce(IContext context)
        {
            if (BuiltIn.Cddr(Cdr) != ClCell.Nil)
            {
                throw new SyntaxError("Invalid function body format");
            }
            var parameters = BuiltIn.First(Cdr).TypeOf<ClCell>();
            if (parameters is null)
            {
                throw new SyntaxError("Invalid function parameters format");
            }
            var invalidParam = BuiltIn.Seq(parameters).FirstOrDefault(it => it.TypeOf<ClSymbol>() is null);
            if (invalidParam is not null)
            {
                throw new SyntaxError($"Binding statement should have {nameof(ClSymbol)} on the left-hand-side");
            }
            var body = BuiltIn.Second(Cdr);
            return context.FromValue(new ClFn(parameters, body, new Env(context.Env)));
        }
    }
}
