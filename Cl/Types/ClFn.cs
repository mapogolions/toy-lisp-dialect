using Cl.Contracts;

namespace Cl.Types
{
    public class ClFn : ClCallable
    {
        public ClFn(ClCell varargs, IClObj body, IEnv lexicalEnv)
        {
            Varargs = varargs;
            Body = body;
            LexicalEnv = lexicalEnv;
        }

        public ClCell Varargs { get; }
        public IClObj Body { get; }
        public IEnv LexicalEnv { get; }

        public override string ToString() => "#<procedure>";
    }
}
