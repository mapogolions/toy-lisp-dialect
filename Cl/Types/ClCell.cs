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
            // ((define x 10) (set x 11)) ~> (nil . (nil . nil))
            var carCtx = Car.Reduce(ctx);
            var cdrCtx = Cdr.Reduce(carCtx);
            return cdrCtx.FromResult(new ClCell(carCtx.Value, cdrCtx.Value));
        }

        public override string ToString() => $"({Car} . {Cdr})";
    }
}
