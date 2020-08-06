namespace Cl.Contracts
{
    public interface IReducable
    {
        IContext Reduce(IContext ctx);
    }
}
