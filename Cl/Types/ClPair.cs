namespace Cl.Types
{
    public class ClPair : IClObj
    {
        public ClPair(IClObj car, IClObj cdr)
        {
            Car = car;
            Cdr = cdr;
        }

        public IClObj Car { get; }
        public IClObj Cdr { get; }
    }
}
