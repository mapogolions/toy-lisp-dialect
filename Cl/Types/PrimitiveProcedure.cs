using System;

namespace Cl.Types
{
    public class PrimitiveProcedure
    {
        private readonly Func<IClObj, IClObj> _fn;
        public PrimitiveProcedure(Func<IClObj, IClObj> fn)
        {
            _fn = fn;
        }

        public IClObj Apply(IClObj obj) => _fn(obj);
    }
}
