using System;
using Cl.Contracts;
using Cl.Errors;
using Cl.Extensions;
using Cl.SpecialForms;

namespace Cl.Types
{
    public class ClCell : ClObj
    {
        public static readonly ClCell Nil = new Nothing();

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
            {
                return new TaggedSpecialForm(tag, Cdr).Reduce(ctx);
            }
            var (value, env) = Car.Reduce(ctx);
            if (value is ClCallable callable)
            {
                return new ApplySpecialForm(callable, Cdr).Reduce(new Context(env));
            }
            throw new SyntaxError("Invalid function call");
        }

        public override string ToString() => $"({Car} . {Cdr})";

        private class Nothing : ClCell
        {
            public Nothing() : base(null, null) { }

            public override ClObj Car => throw new NotImplementedException();
            public override ClObj Cdr => throw new NotImplementedException();
            public override string ToString() => "nil";
            public override IContext Reduce(IContext ctx) => ctx.FromValue(this);
        }
    }
}
