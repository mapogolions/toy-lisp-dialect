using System;
using Cl.Contracts;
using Cl.SpecialForms;

namespace Cl.Types
{
    public class ClCell : IClObj
    {
        public static ClCell Nil = new Nothing(null, null);

        public ClCell(IClObj car, IClObj cdr)
        {
            Car = car;
            Cdr = cdr;
        }

        public virtual IClObj Car { get; }
        public virtual IClObj Cdr { get; }

        public virtual IContext Reduce(IContext ctx)
        {
            if (Car is ClSymbol tag)
                return new TaggedSpecialForm(tag, Cdr).Reduce(ctx);
            var (obj, env) = Car.Reduce(ctx);
            if (obj is ClCallable callable)
                return new ApplySpecialForm(callable, Cdr).Reduce(new Context(env));
            throw new InvalidOperationException();
        }

        public override string ToString() => $"({Car} . {Cdr})";

        private class Nothing : ClCell
        {
            internal Nothing(IClObj car, IClObj cdr) : base(car, cdr) { }

            public override IClObj Car => throw new InvalidOperationException();
            public override IClObj Cdr => throw new InvalidOperationException();
            public override string ToString() => "nil";
            public override IContext Reduce(IContext ctx) => ctx.FromResult(this);
        }
    }
}
