using System;
using System.Collections.Generic;
using Cl.Types;

namespace Cl.Contracts
{
    public interface IReader : IDisposable
    {
        IEnumerable<ClObj> Read();
    }
}
