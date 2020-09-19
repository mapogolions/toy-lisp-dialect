using Cl.Types;

namespace Cl.Contracts
{
    public interface IContext
    {
        IEnv Env { get; }
        ClObj Value { get; }

        IContext FromResult(ClObj result);
        IContext FromEnv(IEnv env);

        void Deconstruct(out ClObj result, out IEnv env);
    }
}
