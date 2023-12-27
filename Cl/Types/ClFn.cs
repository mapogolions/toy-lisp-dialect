namespace Cl.Types
{
    public class ClFn : ClCallable
    {
        public ClFn(ClCell varargs, ClObj body, IEnv scope)
        {
            Varargs = varargs;
            Body = body;
            HasBeenCreatedWithinScope = scope;
        }

        public ClCell Varargs { get; }
        public ClObj Body { get; }
        public IEnv HasBeenCreatedWithinScope { get; }

        public override string ToString() => "#<procedure>";
    }
}
