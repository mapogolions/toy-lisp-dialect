namespace Cl.Types
{
    public class ClFn : ClCallable
    {
        public ClFn(ClCell varargs, ClObj body, IEnv lexicalEnv)
        {
            Parameters = varargs;
            Body = body;
            LexicalEnv = lexicalEnv;
        }

        public ClCell Parameters { get; }
        public ClObj Body { get; }
        public IEnv LexicalEnv { get; }

        public override string ToString() => "#<procedure>";
    }
}
