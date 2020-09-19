using Cl.Contracts;

namespace Cl.Types
{
    public class ClObj : IReducable
    {
        public virtual IContext Reduce(IContext ctx) => ctx.FromResult(this);
    }
}
