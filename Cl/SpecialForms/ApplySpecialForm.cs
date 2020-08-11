using System.Linq;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class ApplySpecialForm : ClCell
    {
        internal ApplySpecialForm(ClFn car, IClObj cdr) : base(car, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var fn = Car as ClFn;
            var args = Cdr.CastOrThrow<ClCell>("Invalid function call");
            var (flipped, env) = BuiltIn.Seq(args)
                .Aggregate<IClObj, IContext>(
                    ctx.FromResult(Nil.Given),
                    (ctx, arg) => {
                        var values = ctx.Value;
                        var (value, env) = arg.Reduce(ctx);
                        return new Context(new ClCell(value, values), env);
                    });
            BuiltIn
                .Seq(fn.Varargs)
                .ZipIfBalanced(BuiltIn.Seq(flipped).Reverse())
                .ForEach(pair => fn.LexicalEnv.Bind(pair.First as ClSymbol, pair.Second));
            var (result, _) = fn.Body.Reduce(new Context(fn.LexicalEnv));
            return ctx.FromResult(result);
        }
    }
}
