using Cl.Contracts;

namespace Cl.Types
{
    public class ClFn : IClObj
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

        public IContext Reduce(IContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString() => "#<procedure>";
    }
}
