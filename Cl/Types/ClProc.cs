namespace Cl.Types
{
    public class ClProc : IClObj
    {
        public ClProc(ClCell varargs, IClObj body)
        {
            Varargs = varargs;
            Body = body;
        }

        public ClCell Varargs { get; }
        public IClObj Body { get; }
    }
}
