namespace Cl.Types
{
    public class ClPair : IClObj
    {
        public IClObj Car { get; set; }
        public IClObj Cdr { get; set; }
    }
}
