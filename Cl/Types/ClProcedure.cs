namespace Cl.Types
{
    public class ClProcedure : IClObj
    {
        public ClProcedure(ClCell varargs, IClObj body)
        {
            Varargs = varargs;
            Body = body;
        }

        public ClCell Varargs { get; }
        public IClObj Body { get; }
    }
}
