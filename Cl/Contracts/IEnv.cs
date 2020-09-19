using System.Collections.Generic;
using Cl.Types;

namespace Cl.Contracts
{
    public interface IEnv
    {
        bool Bind(ClSymbol identifier, ClObj obj);
        bool Bind(IEnumerable<ClObj> identifiers, IEnumerable<ClObj> values);
        ClObj Lookup(ClSymbol indentifier);
        bool Assign(ClSymbol identifier, ClObj obj);
    }
}
