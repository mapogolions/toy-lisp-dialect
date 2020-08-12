using Cl.Contracts;

namespace Cl.Types
{
    public class NativeFn : IClObj
    {
        public NativeFn(IClObj[] varagrs, ParamsFunc<IClObj, IClObj> fn)
        {
            Varags = varagrs;
            Fn = fn;
        }

        public IClObj[] Varags { get; }
        public ParamsFunc<IClObj, IClObj> Fn { get; }

        public IContext Reduce(IContext ctx) => ctx.FromResult(Fn(Varags));

        public override string ToString() => "#<procedure>";
    }
}
