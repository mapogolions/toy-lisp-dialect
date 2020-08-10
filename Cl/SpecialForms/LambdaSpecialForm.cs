using System;
using System.Linq;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class LambdaSpecialForm : BaseSpecialForm
    {
        internal LambdaSpecialForm(IClObj cdr) : base(ClSymbol.Lambda, cdr) { }

        public override IContext Reduce(IContext context)
        {
            if (BuiltIn.Cddr(Cdr) != Nil.Given)
                throw new InvalidOperationException(Errors.Eval.InvalidLambdaBody);
            var parameters = BuiltIn.First(Cdr)
                .CastOrThrow<ClCell>(Errors.Eval.InvalidLambdaParameters);
            var hasUnsupportBinding = BuiltIn.Seq(parameters).Any(it => it.TypeOf<ClSymbol>() is null);
            if (hasUnsupportBinding)
                throw new InvalidOperationException(Errors.BuiltIn.UnsupportBinding);
            var body = BuiltIn.Second(Cdr);
            return context.FromResult(new ClFn(parameters, body, null));
        }
    }
}
