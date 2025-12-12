using Cl.Types;

namespace Cl;

public class Context(ClObj value, IEnv env) : ClObj, IContext
{
    public ClObj Value { get; } = value;
    public IEnv Env { get; } = env;

    public Context(IEnv env) : this(ClCell.Nil, env) { }

    public Context() : this(new Env()) { }
}
