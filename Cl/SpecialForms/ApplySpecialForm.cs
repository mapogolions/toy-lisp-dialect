using Cl.Errors;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    public class ApplySpecialForm : ClCell
    {
        private readonly ClSymbol _symbol;

        public ApplySpecialForm(ClSymbol symbol, ClCallable car, ClObj cdr) : base(car, cdr)
        {
            _symbol = symbol;
        }

        public override IContext Reduce(IContext ctx)
        {
            var (args, env) = EvalArgs(ctx);
            if (Car is NativeFn nativeFn)
            {
                // check if this is a predefined `read` function, if it is then implicitly pass the context as the last argument
                args = ClSymbol.Read.Equals(_symbol) && ctx is Context c ? args.Append(c) : args;
                var result = nativeFn.Apply(args.ToArray());
                return new Context(result, env);
            }
            var fn = (ClFn) Car;
            var lexicalEnv = new Env(fn.Env);
            lexicalEnv.Bind(BuiltIn.Seq(fn.Parameters), args);
            var (value, _) = fn.Body.Reduce(new Context(lexicalEnv));
            return ctx.FromValue(value);
        }

        private (IEnumerable<ClObj>, IEnv) EvalArgs(IContext ctx)
        {
            if (Cdr is not ClCell expressions)
                throw new SyntaxError("Invalid function call");

            var (reversedArgs, env) = BuiltIn.Seq(expressions)
                .Aggregate<ClObj, IContext>(new Context(ctx.Env),
                    (ctx, expression) => {
                        var acc = ctx.Value;
                        var (value, env) = expression.Reduce(ctx);
                        return new Context(new ClCell(value, acc), env);
                    });
            return (BuiltIn.Seq(reversedArgs).Reverse(), env);
        }
    }
}
