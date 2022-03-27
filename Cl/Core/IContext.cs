using Cl.Types;

namespace Cl.Core
{
    public interface IContext
    {
        IEnv Env { get; }
        ClObj Value { get; }
    }
}
