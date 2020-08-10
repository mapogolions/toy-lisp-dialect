using System.Linq;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class ApplySpecialForm : BaseSpecialForm
    {
        internal ApplySpecialForm(ClSymbol car, IClObj cdr) : base(car, cdr) { }

        public override IContext Reduce(IContext context)
        {
            // var obj = context.Env.Lookup(Car);
            // var proc = obj.CastOrThrow<ClFn>($"Function with {Car} name doesn't exist");
            // var args = Cdr.CastOrThrow<ClCell>("Invalid function call");
            //     var values = BuiltIn.Seq(args)
            //     .Aggregate<IClObj, IContext>(context.FromResult(Nil.Given), (seed, x) => {
            //         var evaluatedArgs = seed.Result;
            //         var newContext = x.Reduce(seed);
            //         return new Context(new ClCell(newContext.Result, evaluatedArgs), newContext.Env);
            //     });
            return null;
        }
    }
}
