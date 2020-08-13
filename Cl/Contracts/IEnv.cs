using System.Collections.Generic;
using Cl.Types;

namespace Cl.Contracts
{
    public interface IEnv
    {
        bool Bind(ClSymbol identifier, IClObj obj);
        bool Bind(IEnumerable<IClObj> identifiers, IEnumerable<IClObj> values);
        IClObj Lookup(ClSymbol indentifier);
        bool Assign(ClSymbol identifier, IClObj obj);
    }
}
