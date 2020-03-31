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

        public override string ToString() => $"({Car} {Cdr})";
    }
}
