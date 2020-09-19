using Cl.Types;

namespace Cl.Contracts
{
    public class Context : IContext
    {
        public ClObj Value { get; }
        public IEnv Env { get; }

        public Context(ClObj result, IEnv env)
        {
            Value = result;
            Env = env;
        }

        public Context(IEnv env) : this(ClCell.Nil, env) { }

        public Context() : this(new Env()) { }

        public IContext FromResult(ClObj result) => new Context(result, Env);

        public IContext FromEnv(IEnv env) => new Context(Value, env);

        public void Deconstruct(out ClObj result, out IEnv env)
        {
            result = Value;
            env = Env;
        }
    }
}
