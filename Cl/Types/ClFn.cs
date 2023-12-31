namespace Cl.Types
{
    public class ClFn : ClCallable
    {
        public ClFn(ClCell parameters, ClObj body, IEnv env)
        {
            Parameters = parameters;
            Body = body;
            Env = env;
        }

        public ClCell Parameters { get; }
        public ClObj Body { get; }
        public IEnv Env { get; }

        public override string ToString() => "#<procedure>";
    }
}
