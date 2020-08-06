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

        public virtual IContext Reduce(IContext ctx) => Car is ClSymbol tag
            ? new BaseSpecialForm(tag, Cdr).Reduce(ctx)
            : throw new InvalidOperationException("Invalid function call");

        public override string ToString() => $"({Car} . {Cdr})";
    }
}
