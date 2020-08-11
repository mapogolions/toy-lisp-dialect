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
}
