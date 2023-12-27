namespace Cl.Types
{
    public class NativeFn : ClCallable
    {
        private readonly VarArgsDelegate<ClObj, ClObj> _fn;

        public NativeFn(VarArgsDelegate<ClObj, ClObj> fn, int arity = 1)
        {
            _fn = fn;
            Arity = arity;
        }

        public int Arity { get; }

        public ClObj Apply(params ClObj[] items) => _fn(items);

        public override string ToString() => "#<native>";
    }
}
