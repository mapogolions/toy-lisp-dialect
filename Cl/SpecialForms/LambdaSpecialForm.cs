using System;
using System.Linq;
using Cl.Contracts;
using Cl.Exceptions;
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
                throw new InvalidOperationException(Errors.Eval.InvalidLambdaBodyFormat);
            var parameters = BuiltIn.First(Cdr)
                .CastOrThrow<ClCell>(Errors.Eval.InvalidLambdaParametersFormat);
            var invalidParam = BuiltIn.Seq(parameters).FirstOrDefault(it => it.TypeOf<ClSymbol>() is null);
            if (invalidParam is not null)
                throw new InvalidBindingException(invalidParam.GetType().Name);
            var body = BuiltIn.Second(Cdr);
            return context.FromValue(new ClFn(parameters, body, new Env(context.Env)));
        }
    }
}
