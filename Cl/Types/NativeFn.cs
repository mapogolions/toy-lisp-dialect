using System;
using Cl.Contracts;

namespace Cl.Types
{
    public class NativeFn : ClCallable
    {
        private readonly ParamsFunc<IClObj, IClObj> _fn;
        public NativeFn(ParamsFunc<IClObj, IClObj> fn) => _fn = fn;

        public IClObj Call(params IClObj[] items) => _fn(items);

        public override string ToString() => "#<native>";
    }
}
