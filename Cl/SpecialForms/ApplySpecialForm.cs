using System.Linq;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class ApplySpecialForm : BaseSpecialForm
    {
        internal ApplySpecialForm(ClSymbol car, IClObj cdr) : base(car, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var fn = ctx.Env.Lookup(Tag).CastOrThrow<ClFn>($"Function with {Tag} name doesn't exist");
            var args = Cdr.CastOrThrow<ClCell>("Invalid function call");
            var (values, env) = BuiltIn.Seq(args)
                .Aggregate<IClObj, IContext>(
                    ctx.FromResult(Nil.Given),
                    (ctx, arg) => {
                        var values = ctx.Value;
                        var (value, env) = arg.Reduce(ctx);
                        return new Context(new ClCell(value, values), env);
                    });

            return null;
        }
    }
}
