using System;
using Cl.Contracts;
using Cl.SpecialForms;

namespace Cl.Types
{
    public class ClCell : IClObj
    {
        public ClCell(IClObj car, IClObj cdr)
        {
            Car = car;
            Cdr = cdr;
        }

        public IClObj Car { get; }
        public IClObj Cdr { get; }

        public virtual IContext Reduce(IContext ctx)
        {
            if (Car is ClSymbol tag)
                return new TaggedSpecialForm(tag, Cdr).Reduce(ctx);
            var (obj, env) = Car.Reduce(ctx);
            if (obj is ClFn fn)
                return new ApplySpecialForm(fn, Cdr).Reduce(new Context(env));
            throw new InvalidOperationException();
        }

        public override string ToString() => $"({Car} . {Cdr})";
    }
}
