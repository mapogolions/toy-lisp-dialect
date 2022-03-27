using System;
using System.Collections.Generic;
using Cl.Types;

namespace Cl.Core
{
    public interface IReader : IDisposable
    {
        IEnumerable<ClObj> Read();
    }
}
