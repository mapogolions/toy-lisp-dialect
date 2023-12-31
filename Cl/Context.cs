using Cl.Types;

namespace Cl
{
    public class Context : ClObj, IContext
    {
        public ClObj Value { get; }
        public IEnv Env { get; }

        public Context(ClObj value, IEnv env)
        {
            Value = value;
            Env = env;
        }

        public Context(IEnv env) : this(ClCell.Nil, env) { }

        public Context() : this(new Env()) { }
    }
}
