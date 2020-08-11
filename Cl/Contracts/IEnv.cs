using Cl.Types;

namespace Cl.Contracts
{
    public interface IEnv
    {
        bool Bind(ClSymbol identifier, IClObj obj);
        IClObj Lookup(ClSymbol indentifier);
        bool Assign(ClSymbol identifier, IClObj obj);
        IEnv New();
    }
}
