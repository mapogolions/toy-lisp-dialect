using Cl.Types;

namespace Cl;

public interface IContext
{
    IEnv Env { get; }
    ClObj Value { get; }
}
