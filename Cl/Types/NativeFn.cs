using System;
using Cl.Contracts;

namespace Cl.Types
{
    public class NativeFn : ClCallable
    {
        public NativeFn(ParamsFunc<IClObj, IClObj> fn)
        {
            Fn = fn;
        }

        public ParamsFunc<IClObj, IClObj> Fn { get; }

        public override string ToString() => "#<native>";
    }
}
