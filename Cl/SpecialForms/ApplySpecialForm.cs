using System;
using System.Collections.Generic;
using System.Linq;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class ApplySpecialForm : ClCell
    {
        internal ApplySpecialForm(ClCallable car, ClObj cdr) : base(car, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var (args, env) = EvalArgs(ctx);
            if (Car is NativeFn nativeFn)
            {
                var result = nativeFn.Apply(args.ToArray());
                return new Context(result, env);
            }
            var fn = (ClFn) Car;
            var lexicalEnv = new Env(fn.HasBeenCreatedWithinScope);
            lexicalEnv.Bind(BuiltIn.Seq(fn.Varargs), args);
            var (value, _) = fn.Body.Reduce(new Context(lexicalEnv));
            return ctx.FromValue(value);
        }

        private (IEnumerable<ClObj>, IEnv) EvalArgs(IContext ctx)
        {
            var obj = Cdr.CastOrThrow<ClCell>(Errors.Eval.InvalidFunctionCall);
            var (reversedArgs, env) = BuiltIn.Seq(obj)
                .Aggregate<ClObj, IContext>(new Context(ctx.Env),
                    (ctx, expr) => {
                        var acc = ctx.Value;
                        var (value, env) = expr.Reduce(ctx);
                        return new Context(new ClCell(value, acc), env);
                    });
            return (BuiltIn.Seq(reversedArgs).Reverse(), env);
        }
    }
}
