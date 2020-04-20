namespace Cl.Types
{
    public class ClProcedure : IClObj
    {
        public ClProcedure(ClCell varargs, IClObj body, IEnv lexicalEnvironment)
        {
            Varargs = varargs;
            Body = body;
            LexicalEnvironment = lexicalEnvironment;
        }

        public ClCell Varargs { get; }
        public IClObj Body { get; }
        public IEnv LexicalEnvironment { get; }
    }
}
