namespace Cl.Types;

public class ClFn(ClCell parameters, ClObj body, IEnv env) : ClCallable
{
    public ClCell Parameters { get; } = parameters;
    public ClObj Body { get; } = body;
    public IEnv Env { get; } = env;

    public override string ToString() => "#<procedure>";
}
