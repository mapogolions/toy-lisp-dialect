using System;
using Cl.Contracts;

namespace Cl.Types
{
    public class NativeFn : ClCallable
    {
        public NativeFn(ParamsFunc<IClObj, IClObj> fn)
        {
            Call = fn;
        }

        public ParamsFunc<IClObj, IClObj> Call { get; }

        public override string ToString() => "#<native>";
    }
}
