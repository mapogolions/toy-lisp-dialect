namespace Cl.Types
{
    public class ClFn : ClCallable
    {
        public ClFn(ClCell parameters, ClObj body, IEnv lexicalEnv)
        {
            Parameters = parameters;
            Body = body;
            LexicalEnv = lexicalEnv;
        }

        public ClCell Parameters { get; }
        public ClObj Body { get; }
        public IEnv LexicalEnv { get; }

        public override string ToString() => "#<procedure>";
    }
}
