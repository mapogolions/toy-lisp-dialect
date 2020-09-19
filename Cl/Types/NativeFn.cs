using System;
using Cl.Contracts;

namespace Cl.Types
{
    public class NativeFn : ClCallable
    {
        private readonly ParamsFunc<ClObj, ClObj> _fn;
        public NativeFn(ParamsFunc<ClObj, ClObj> fn) => _fn = fn;

        public ClObj Call(params ClObj[] items) => _fn(items);

        public override string ToString() => "#<native>";
    }
}
