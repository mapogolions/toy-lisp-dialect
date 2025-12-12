namespace Cl.Types;

public class NativeFn(VarArgsDelegate<ClObj, ClObj> fn, int arity = 1) : ClCallable
{
    public int Arity { get; } = arity;

    public ClObj Apply(params ClObj[] items) => fn(items);

    public override string ToString() => "#<native>";
}
