namespace Cl.Core
{
    public interface IReducable
    {
        IContext Reduce(IContext ctx);
    }
}
