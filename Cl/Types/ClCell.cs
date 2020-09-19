using System;
using Cl.Contracts;
using Cl.SpecialForms;

namespace Cl.Types
{
    public class ClCell : ClObj
    {
        public static ClCell Nil = new Nothing(null, null);

        public ClCell(ClObj car, ClObj cdr)
        {
            Car = car;
            Cdr = cdr;
        }

        public virtual ClObj Car { get; }
        public virtual ClObj Cdr { get; }

        public override IContext Reduce(IContext ctx)
        {
            if (Car is ClSymbol tag)
                return new TaggedSpecialForm(tag, Cdr).Reduce(ctx);
            var (obj, env) = Car.Reduce(ctx);
            if (obj is ClCallable callable)
                return new ApplySpecialForm(callable, Cdr).Reduce(new Context(env));
            throw new InvalidOperationException(Errors.Eval.InvalidFunctionCall);
        }

        public override string ToString() => $"({Car} . {Cdr})";

        private class Nothing : ClCell
        {
            internal Nothing(ClObj car, ClObj cdr) : base(car, cdr) { }

            public override ClObj Car => throw new InvalidOperationException();
            public override ClObj Cdr => throw new InvalidOperationException();
            public override string ToString() => "nil";
            public override IContext Reduce(IContext ctx) => ctx.FromResult(this);
        }
    }
}
