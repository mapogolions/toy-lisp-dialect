using Cl.Contracts;

namespace Cl.Types
{
    public class ClCallable : IClObj
    {
        public IContext Reduce(IContext ctx) => ctx.FromResult(this);
    }
}
