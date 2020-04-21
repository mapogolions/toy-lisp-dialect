namespace Cl.Types
{
    public class ClProcedure : IClObj
    {
        public ClProcedure(ClCell varargs, IClObj body, IEnv lexicalEnv)
        {
            Varargs = varargs;
            Body = body;
            LexicalEnv = lexicalEnv;
        }

        public ClCell Varargs { get; }
        public IClObj Body { get; }
        public IEnv LexicalEnv { get; }
    }
}
