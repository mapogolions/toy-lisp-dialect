using Cl.Types;

namespace Cl.Contracts
{
    public interface IContext
    {
        IEnv Env { get; }
        IClObj Result { get; }

        IContext FromResult(IClObj result);
        IContext FromEnv(IEnv env);
    }

    public class Context : IContext
    {
        public IClObj Result { get; }
        public IEnv Env { get; }

        public Context(IClObj result, IEnv env)
        {
            Result = result;
            Env = env;
        }

        public Context() : this(Nil.Given, new Env()) { }

        public IContext FromResult(IClObj result) => new Context(result, Env);

        public IContext FromEnv(IEnv env) => new Context(Result, env);
    }
}
