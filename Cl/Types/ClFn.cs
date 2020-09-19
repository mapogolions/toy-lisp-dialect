using Cl.Contracts;

namespace Cl.Types
{
    public class ClFn : ClCallable
    {
        public ClFn(ClCell varargs, ClObj body, IEnv lexicalEnv)
        {
            Varargs = varargs;
            Body = body;
            LexicalEnv = lexicalEnv;
        }

        public ClCell Varargs { get; }
        public ClObj Body { get; }
        public IEnv LexicalEnv { get; }

        public override string ToString() => "#<procedure>";
    }
}
