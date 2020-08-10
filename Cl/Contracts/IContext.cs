using Cl.Types;

namespace Cl.Contracts
{
    public interface IContext
    {
        IEnv Env { get; }
        IClObj Value { get; }

        IContext FromResult(IClObj result);
        IContext FromEnv(IEnv env);

        void Deconstruct(out IClObj result, out IEnv env);
    }

    public class Context : IContext
    {
        public IClObj Value { get; }
        public IEnv Env { get; }

        public Context(IClObj result, IEnv env)
        {
            Value = result;
            Env = env;
        }

        public Context(IEnv env) : this(Nil.Given, env) { }

        public Context() : this(new Env()) { }

        public IContext FromResult(IClObj result) => new Context(result, Env);

        public IContext FromEnv(IEnv env) => new Context(Value, env);

        public void Deconstruct(out IClObj result, out IEnv env)
        {
            result = Value;
            env = Env;
        }
    }
}
