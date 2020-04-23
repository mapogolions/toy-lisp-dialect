using System;

namespace Cl.Types
{
    public class NativeFn : IClObj
    {
        private readonly Func<IClObj, IClObj> _fn;

        public NativeFn(Func<IClObj, IClObj> fn)
        {
            _fn = fn;
        }

        public IClObj Apply(IClObj obj) => _fn(obj);

        public override string ToString() => "#<procedure>";
    }
}
