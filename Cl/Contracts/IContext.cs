using Cl.Types;

namespace Cl.Contracts
{
    public interface IContext
    {
        IEnv Env { get; }
        ClObj Value { get; }
    }
}
