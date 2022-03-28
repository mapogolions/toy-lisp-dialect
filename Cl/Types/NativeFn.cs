using Cl;

namespace Cl.Types
{
    public class NativeFn : ClCallable
    {
        private readonly VarargDelegate<ClObj, ClObj> _fn;
        public NativeFn(VarargDelegate<ClObj, ClObj> fn) => _fn = fn;

        public ClObj Apply(params ClObj[] items) => _fn(items);

        public override string ToString() => "#<native>";
    }
}
